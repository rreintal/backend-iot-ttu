using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using ContentType = BLL.DTO.V1.ContentType;
using News = BLL.DTO.V1.News;


namespace App.BLL.Services;

public class NewsService : BaseEntityService<News, Domain.News, INewsRepository>, INewsService, INewsRepositoryCustom<News>
{
    private IAppUOW Uow { get; set; }
    private IMapper _mapper { get; }
    
    private IThumbnailService ThumbnailService { get; }
    
    // need Add, Remove jne on basic operationid
    // kui vaja tagastada DTO siis seda tehakse custom meetoditega!!
    public NewsService(IAppUOW uow, IMapper<News, Domain.News> mapper, IMapper autoMapper, IThumbnailService thumbnailService) : base(uow.NewsRepository, mapper)
    {
        Uow = uow;
        _mapper = autoMapper;
        ThumbnailService = thumbnailService;
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
        
        // Add thumbnail
        // TODO - check if its valid!
        try
        {
            domainObject.ThumbnailImage = ThumbnailService.Compress(domainObject.Image);
        }
        catch (Exception e)
        {
            domainObject.ThumbnailImage = "IMAAGE COMPRESSING THREW AND EXCEPTION!";
        }
        

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

    public async Task<IEnumerable<News>> AllAsyncFiltered(int? page, int? size)
    {
        return (await Uow.NewsRepository.AllAsyncFiltered(page, size)).Select(e => _mapper.Map<News>(e));
    }
    

    public async Task<UpdateNews> UpdateNews(UpdateNews entity)
    {
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateNews>(entity);
        await Uow.NewsRepository.Update(dalEntity);
        await Uow.SaveChangesAsync();
        return entity;
    }

    public async Task<News> FindByIdAllTranslationsAsync(Guid id)
    {
         var entity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(id);
         return _mapper.Map<News>(entity);
    }
}
/*
 *
{
  "id": "5ae4fd34-1400-45c2-9bbf-f5b209f38091",
  "author": "RICHARD REINTAL UUS",
  "body": [
    {
      "value": "UUS BODY EESTI",
      "culture": "et"
    },
{
      "value": "NEW BODY Eng",
      "culture": "en"
    }
  ],
  "title": [
    {
      "value": "UUS TITLE EST",
      "culture": "et"
    },
{
      "value": "NEW TITLE ENG",
      "culture": "en"
    }
  ],
  "image": "abcdef",
  "topicAreas": [
    {
      "id": "4819415b-8886-45c4-8981-8c9592b7757f"
    },
    { "id" : "c1999007-1a3a-47a1-bc7b-951ec1940353" }
  ]
}


d981490b-9aec-49f1-bc4e-73d7ae5d4862
*/

