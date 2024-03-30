using App.BLL.Contracts;
using App.DAL.Contracts;
using App.DAL.EF.DbExceptions;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;


namespace App.BLL.Services;

public class TopicAreaService : BaseEntityService<TopicArea, Domain.TopicArea, ITopicAreaRepository>, ITopicAreaService
{
    // TODO - see peaks andma BLL dto tegelt!
    private IAppUOW Uow { get; set; }
    
    public TopicAreaService(IAppUOW uow, IMapper<TopicArea, Domain.TopicArea> mapper) : base(uow.TopicAreaRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<TopicArea>> AllAsync()
    {
        return (await Uow.TopicAreaRepository.AllAsync()).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<TopicArea>> AllAsync(string? languageCulture)
    {
        return (await Uow.TopicAreaRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }
    
    public Task<TopicArea?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }


    public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(string languageCulture)
    {
        return Uow.TopicAreaRepository.GetTopicAreasWithCount(languageCulture);
    }

    public override TopicArea Remove(TopicArea entity)
    {
        return base.Remove(entity);
    }
}