using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1.FilterObjects;
using Public.DTO.V1.Mappers;

namespace App.BLL.Services;

public class TopicAreaService : BaseEntityService<TopicArea, Domain.TopicArea, ITopicAreaRepository>, ITopicAreaService
{
    // TODO - see peaks andma BLL dto tegelt!
    private IAppUOW Uow { get; set; }
    
    public TopicAreaService(IAppUOW uow, IMapper<TopicArea, Domain.TopicArea> mapper) : base(uow.TopicAreaRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<TopicAreaWithCount>> GetTopicAreaWithCount(TopicAreaCountFilter filter)
    {
        
        var domainObjects = await Uow.TopicAreaRepository.GetHasTopicArea(filter);
        
        var bufferDict = new Dictionary<Guid, TopicAreaWithCount>();
        var result = new List<TopicAreaWithCount>();
        
        foreach (var hta in domainObjects)
        {
            if (hta.TopicArea!.HasParentTopicArea())
            {
                var parent = hta.TopicArea!.ParentTopicArea;
                var child = new TopicAreaWithCount()
                {
                    Id = hta.TopicArea.Id,
                    Count = 1,
                    Name = hta.TopicArea.GetName()
                };
                
                if (bufferDict.ContainsKey(parent!.Id))
                {
                    var existingParentDto = bufferDict[parent.Id];
                    // if child already exists, then just increment count
                    if (ChildAlreadyExists(existingParentDto, child.Id))
                    {
                        var existingChild = existingParentDto.Children.Where(x => x.Id == child.Id).First();
                        existingParentDto.Count++;
                        existingChild.Count++;
                    }
                    else
                    {
                        Console.WriteLine("Adding child!");
                        // if child does not exist then add!
                        existingParentDto.Children!.Add(child);
                        existingParentDto.Count++;   
                    }
                }
                else
                {
                    // If its not in the dictionary, then add
                    var parentDto = new TopicAreaWithCount()
                    {
                        Id = parent.Id,
                        Name = parent.GetName(),
                        Count = 1,
                        Children = new List<TopicAreaWithCount>()
                        {
                            child
                        }
                    };
                    
                    bufferDict.Add(parent.Id, parentDto);
                }
            }
            
        }

        foreach (var item in bufferDict)
        {
            result.Add(item.Value);
        }

        return result;
    }

    public async Task<IEnumerable<TopicArea>> GetTopicAreasWithTranslations()
    {
        return (await Uow.TopicAreaRepository.GetTopicAreasWithAllTranslations()).Select(e => Mapper.Map(e));
    }


    private bool ChildAlreadyExists(TopicAreaWithCount parent, Guid childId)
    {
        // TODO - optimize
        foreach (var children in parent.Children!)
        {
            if (children.Id == childId)
            {
                return true;
            }
        }
        return false;
    }
}