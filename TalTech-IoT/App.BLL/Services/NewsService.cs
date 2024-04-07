using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using Microsoft.IdentityModel.Tokens;
using Public.DTO;
using Public.DTO.ApiExceptions;
using ContentType = BLL.DTO.V1.ContentType;
using ImageResource = BLL.DTO.V1.ImageResource;
using News = BLL.DTO.V1.News;


namespace App.BLL.Services;

public class NewsService : BaseEntityService<News, Domain.News, INewsRepository>, INewsService
{
    private IAppUOW Uow { get; }
    private IMapper _mapper { get; }
    private IImageStorageService _imageStorageService { get; }
    private IThumbnailService _thumbnailService { get; }
    
    // need Add, Remove jne on basic operationid
    // kui vaja tagastada DTO siis seda tehakse custom meetoditega!!
    public NewsService(IAppUOW uow, IMapper<News, Domain.News> mapper, IMapper autoMapper, IThumbnailService thumbnailService, IImageStorageService imageStorageService) : base(uow.NewsRepository, mapper)
    {
        Uow = uow;
        _mapper = autoMapper;
        _thumbnailService = thumbnailService;
        _imageStorageService = imageStorageService;
    }
    
    
    public async Task<News?> UpdateAsync(News entity)   
    {
        var isDuplicateTopicAreas = entity.TopicAreas
            .GroupBy(e => e.Id)
            .Any(group => group.Count() > 1);

        if (isDuplicateTopicAreas)
        {
            throw new TopicAreasNotUnique();
        }
        
        
        var existingEntity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(entity.Id);
        if (existingEntity == null)
        {
            return null;
        }

        entity.ThumbnailImage = existingEntity.ThumbnailImage;
        if (entity.Image != null)
        {
            if (_imageStorageService.IsBase64(entity.Image))
            {
                try
                {
                    entity.ThumbnailImage = _thumbnailService.Compress(entity.Image);
                }
                catch (Exception e)
                {
                    entity.ThumbnailImage = "Thumbnailservice threw an exception";
                }
            }
        }

        entity.ImageResources = existingEntity.ImageResources.Select(e => new ImageResource()
        {
            NewsId = e.NewsId,
            Link = e.Link
        }).ToList();
        
        
        var updateResult = _imageStorageService.ProccessUpdate(entity);
        if (updateResult != null)
        {
            entity.ImageResources = entity.ImageResources
                .Where(image => updateResult.DeletedLinks == null || !updateResult.DeletedLinks.Contains(image.Link))
                .ToList();

            // Add the SavedLinks
            if (updateResult.SavedLinks != null)
            {
                foreach (var link in updateResult.SavedLinks)
                {
                    entity.ImageResources.Add(new global::BLL.DTO.V1.ImageResource() { Link = link, NewsId = entity.Id});
                }
            }

            if (updateResult.DeletedLinks != null)
            {
                DeleteContent data = new DeleteContent();
                data.Links = updateResult.DeletedLinks;
                _imageStorageService.ProcessDelete(data);
            }
        }
        
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var updatedDalEntity =  await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
        return bllEntity;
    }
    
    
    /*
    public async Task<News?> UpdateAsync(News entity)
    {
        var existingEntity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(entity.Id);
        if (existingEntity == null)
        {
            return null;
        }

        // Business logic to process the thumbnail image
        var thumbNail = entity.ThumbnailImage == null ? existingEntity.ThumbnailImage : entity.ThumbnailImage;
        _imageStorageService.ProccessUpdate(entity);

        // Get IDs of TopicAreas currently associated with the News entity
        var currentTopicAreaIds = existingEntity.HasTopicAreas.Select(hta => hta.TopicAreaId).ToList();

        // Determine which TopicArea associations need to be removed
        var toRemove = existingEntity.HasTopicAreas
            .Where(hta => !entity.TopicAreas.Select(ta => ta.Id).Contains(hta.TopicAreaId))
            .ToList();

        // Remove the determined TopicArea associations
        foreach (var removeItem in toRemove)
        {
            existingEntity.HasTopicAreas.Remove(removeItem);
        }

        // Find new TopicAreaIds to add (those not already associated with the News entity)
        var toAdd = entity.TopicAreas.Select(ta => ta.Id).Except(currentTopicAreaIds).ToList();
        foreach (var addId in toAdd)
        {
            existingEntity.HasTopicAreas.Add(new HasTopicArea
            {
                TopicAreaId = addId,
                NewsId = existingEntity.Id
            });
        }

        // Additional properties update
        existingEntity.Author = entity.Author; // Assuming author needs to be updated here
        
        // Call DAL to persist the changes
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(existingEntity);
        var updatedDalEntity = await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
    
        return bllEntity;
    }
    */

    public async Task<News> AddAsync(News entity)
    {
        var isDuplicateTopicAreas = entity.TopicAreas
            .GroupBy(e => e.Id)
            .Any(group => group.Count() > 1);

        if (isDuplicateTopicAreas)
        {
            throw new TopicAreasNotUnique();
        }
        try
        {
            entity.ThumbnailImage = _thumbnailService.Compress(entity.Image);
        }
        catch (Exception e)
        {
            // TODO: what to do here? this actually should not get to this point
            entity.ThumbnailImage = "IMAGE COMPRESSING THREW AND EXCEPTION!";
        }
        var serviceResult = _imageStorageService.ProccessSave(entity);
        if (serviceResult != null)
        {
            entity.ImageResources = serviceResult.SavedLinks.Select(e => new ImageResource()
            {
                NewsId = entity.Id,
                Link = e
            }).ToList();
        }
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
            var response = _imageStorageService.ProcessDelete(data);
        
            // What to do if it fails?? notify user?
            if (response == false)
            {
                Console.WriteLine("NewsService: ProcessDelete to CDN failed!");            
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
            entity.ThumbnailImage = _thumbnailService.Compress(entity.Image);
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