using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using ContentType = BLL.DTO.V1.ContentType;
using News = BLL.DTO.V1.News;


namespace App.BLL.Services;

public class NewsService : BaseEntityService<News, Domain.News, INewsRepository>, INewsService
{
    private IAppUOW Uow { get; set; }
    private IMapper _mapper { get; }
    
    // need Add, Remove jne on basic operationid
    // kui vaja tagastada DTO siis seda tehakse custom meetoditega!!
    public NewsService(IAppUOW uow, IMapper<News, Domain.News> mapper, IMapper autoMapper) : base(uow.NewsRepository, mapper)
    {
        Uow = uow;
        _mapper = autoMapper;
    }

    public async Task<IEnumerable<News>> AllAsync()
    {
        return (await Uow.NewsRepository.AllAsync()).Select(x => _mapper.Map<News>(x));
    }

    public async Task<News?> FindAsync(Guid id)
    {
        // domain object
        var item = await Uow.NewsRepository.FindAsync(id);
        return _mapper.Map<News>(item);
    }

    public async Task<List<ContentType>> GetContentTypes()
    {
        var titleContentType = Uow.ContentTypesRepository.FindByName("TITLE");
        var bodyContentType = Uow.ContentTypesRepository.FindByName("BODY");
        
        var body = new ContentType()
        {
            Id = bodyContentType.Id,
            Name = bodyContentType.Name
        };

        var title = new ContentType()
        {
            Id = titleContentType.Id,
            Name = titleContentType.Name
        };
        var types = new List<ContentType>()
        {
            body, title
        };
        return types;
    }

    public News Add(News entity)
    {
        var domainObject = _mapper.Map<App.Domain.News>(entity);
        foreach (var bllTopicArea in entity.TopicAreas)
        {
            var hasTopicAreaId = Guid.NewGuid();
            var hasTopicArea = new App.Domain.HasTopicArea()
            {
                Id = hasTopicAreaId,
                NewsId = domainObject.Id,
                TopicAreaId = bllTopicArea.Id,
            };

            if (domainObject.HasTopicAreas == null)
            {
                domainObject.HasTopicAreas = new List<HasTopicArea>()
                {
                    hasTopicArea
                };
            }
            else
            {
                domainObject.HasTopicAreas.Add(hasTopicArea);
            }
        }

        Uow.NewsRepository.Add(domainObject);
        
        
        return entity;
    }
}