using System.Drawing.Printing;
using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using Microsoft.IdentityModel.Tokens;
using Public.DTO;
using Public.DTO.ApiExceptions;
using ContentType = BLL.DTO.V1.ContentType;
using News = BLL.DTO.V1.News;


namespace App.BLL.Services;

public class NewsService : BaseEntityService<News, Domain.News, INewsRepository>, INewsService
{
    private IAppUOW Uow { get; set; }
    private IMapper _mapper { get; }
    private IImageStorageService _imageStorageService { get; set; }
    
    private IThumbnailService ThumbnailService { get; }
    
    // need Add, Remove jne on basic operationid
    // kui vaja tagastada DTO siis seda tehakse custom meetoditega!!
    public NewsService(IAppUOW uow, IMapper<News, Domain.News> mapper, IMapper autoMapper, IThumbnailService thumbnailService, IImageStorageService imageStorageService) : base(uow.NewsRepository, mapper)
    {
        Uow = uow;
        _mapper = autoMapper;
        ThumbnailService = thumbnailService;
        _imageStorageService = imageStorageService;
    }
    
    public async Task<News?> UpdateAsync(News entity)
    {
        var existingEntity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(entity.Id);
        if (existingEntity == null)
        {
            return null;
        }
        
        
        var thumbNail = entity.ThumbnailImage == null ? existingEntity.ThumbnailImage : entity.ThumbnailImage;

        _imageStorageService.ProccessUpdate(entity);
        // TODO: IMAGERESOURCES
        
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var updatedDalEntity =  await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
        return bllEntity;
        
    }

    public async Task<News> AddAsync(News entity)
    {
        var isDuplicateTopicAreas = entity.TopicAreas
            .GroupBy(e => e.Id)
            .Any(group => group.Count() > 1);

        Console.WriteLine($"IS DUPLICATE: {isDuplicateTopicAreas}");
        if (isDuplicateTopicAreas)
        {
            throw new TopicAreasNotUnique();
        }
        
        try
        {
            entity.ThumbnailImage = ThumbnailService.Compress(entity.Image);
        }
        catch (Exception e)
        {
            // TODO: what to do here? this actually should not get to this point
            entity.ThumbnailImage = "IMAGE COMPRESSING THREW AND EXCEPTION!";
        }
        var serviceResult = _imageStorageService.ProccessSave(entity);
        Console.WriteLine("ImageService result: " + serviceResult);
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var dalResult = await Uow.NewsRepository.AddAsync(dalEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
    }

    public async Task<News> RemoveAsync(News entity)
    {
        var imageResources = (await Uow.NewsRepository.GetImageResources(entity.Id)).Select(e => e.Link);
        var data = new DeleteContent()
        {
            Links = imageResources.ToList()
        };

        if (!data.Links.IsNullOrEmpty())
        {
            var response = _imageStorageService.Delete(data);
        
            // What to do if it fails?? notify user?
            if (response == false)
            {
                Console.WriteLine("NewsService: Delete to CDN failed!");            
            }   
        }
        var dalResult = await Uow.NewsRepository.RemoveAsync(entity.Id);
        return _mapper.Map<News>(dalResult);
    }
    
    public override News Add(News entity)
    {
        // TODO: thumbnail!
        try
        {
            entity.ThumbnailImage = ThumbnailService.Compress(entity.Image);
        }
        catch (Exception e)
        {
            entity.ThumbnailImage = "IMAGE COMPRESSING THREW AND EXCEPTION!";
        }
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var dalResult = Uow.NewsRepository.Add(dalEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
    }

    // OK
    public override async Task<IEnumerable<News>> AllAsync()
    {
        return (await Uow.NewsRepository.AllAsync()).Select(x => _mapper.Map<News>(x));
    }
    
    public override async Task<News?> FindAsync(Guid id)
    {
        var item = await Uow.NewsRepository.FindAsync(id);
        return _mapper.Map<News>(item);
    }

    public async Task<IEnumerable<News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageString)
    {
        // TODO: add this method to common interface w service/repo
        return (await Uow.NewsRepository.AllAsyncFiltered(filterSet, languageString)).Select(e => _mapper.Map<News>(e));
    }
    
    public async Task<News?> FindByIdAllTranslationsAsync(Guid id)
    {
        var entity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(id);
        return _mapper.Map<News>(entity);
    }
    

    public async Task<IEnumerable<News>> AllAsync(string? languageCulture)
    {
        return (await Uow.NewsRepository.AllAsync(languageCulture)).Select(entity => _mapper.Map<News>(entity));
    }

    public async Task<News?> FindAsync(Guid id, string? languageCulture)
    {
        var item = await Uow.NewsRepository.FindAsync(id, languageCulture);
        return _mapper.Map<News>(item);
    }

    public async Task<int> FindNewsTotalCount(Guid? TopicAreaId)
    {
        return await Uow.NewsRepository.FindNewsTotalCount(TopicAreaId);
    }

    public async Task IncrementViewCount(Guid id)
    {
        await Uow.NewsRepository.IncrementViewCount(id);
    }

    // TODO: Move this to another repository
    public async Task<List<ContentType>> GetContentTypes()
    {
        var titleContentType = await Uow.ContentTypesRepository.FindByName(ContentTypes.TITLE);
        var bodyContentType = await Uow.ContentTypesRepository.FindByName(ContentTypes.BODY);
        
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
}