using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using Microsoft.IdentityModel.Tokens;
using Public.DTO;
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
        
        /*
        var bodyEn = ContentHelper.GetContentValue(entity, ContentTypes.BODY, LanguageCulture.ENG);
        var bodyEt = ContentHelper.GetContentValue(entity, ContentTypes.BODY, LanguageCulture.EST);
        var image = entity.Image;
        */
        
        // If not adding the image + Thumbnail image then when updating
        // the 'not used' or more specific 'not in the list when checking for unused images' then it gets deleted
        var thumbNail = entity.ThumbnailImage == null ? existingEntity.ThumbnailImage : entity.ThumbnailImage;

        _imageStorageService.ProccessUpdate(entity);
        /*
        var updateData = new UpdateContent()
        {
            Items = new List<UpdateItem>()
        };
        
        var imageResources = await Uow.NewsRepository.GetImageResources(entity.Id);
        updateData.ExistingImageLinks = imageResources.Select(e => e.Link).ToList();

        var updateBodyEn = new UpdateItem()
        {
            Content = bodyEn,
            Sequence = 0
        };

        var updateBodyEt = new UpdateItem()
        {
            Content = bodyEt,
            Sequence = 1
        };

        var updateImage = new UpdateItem()
        {
            Content = image,
            Sequence = 2,
            IsAlreadyBase64 = true
        };

        var thumbNailImage = new UpdateItem()
        {
            Content = thumbNail,
            Sequence = 3,
            IsAlreadyBase64 = true
        };
        
        updateData.Items.Add(updateBodyEn);
        updateData.Items.Add(updateBodyEt);
        updateData.Items.Add(updateImage);
        updateData.Items.Add(thumbNailImage);

        var updateResult = _imageStorageService.Update(updateData);
        /*
        if (updateResult != null && !updateResult.IsEmpty())
        {
            ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.EST, updateResult.Items.First(e => e.Sequence == 1).Content);
            ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.ENG, updateResult.Items.First(e => e.Sequence == 0).Content);
            entity.Image = updateResult.Items.First(e => e.Sequence == 2).Content;
            entity.ThumbnailImage = updateResult.Items.First(e => e.Sequence == 3).Content;

            x
            // TODO: this is a hack as the BLL entity does not have the IMAGERESOURCES but DOMAIN object has 

            entity.ImageResources = existingEntity.ImageResources.Select(e => _mapper.Map<ImageResource>(e)).ToList();
            var IsAddedLinksEmpty = updateResult.AddedLinks.IsNullOrEmpty();
            if (!IsAddedLinksEmpty)
            {
                // TODO: IMPORTANT- when fetching the object for update we need all these ImageResources!!1
                foreach (var newLink in updateResult.AddedLinks)
                {
                    entity.ImageResources.Add(new ImageResource()
                    {
                        NewsId = entity.Id,
                        Link = newLink 
                    });   
                }
            }

            var IsDeletedLinksEmpty = updateResult.DeletedLinks.IsNullOrEmpty();
            if (!IsDeletedLinksEmpty && updateResult.DeletedLinks != null)
            {
                foreach (var deletedLink in updateResult.DeletedLinks)
                {
                    var itemToRemove = entity.ImageResources.First(e => e.Link == deletedLink);
                    entity.ImageResources.Remove(itemToRemove);
                }
            }
        }
        */
        // TODO: IMAGERESOURCES
        // TODO: IMAGERESOURCES
        // TODO: IMAGERESOURCES
        // TODO: IMAGERESOURCES
        // TODO: IMAGERESOURCES
        
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var updatedDalEntity =  await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
        return bllEntity;
        
    }

    public async Task<News> AddAsync(News entity)
    {
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
        
        
        // User input content
        /*
        var bodyEn = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.ENG);
        var bodyEt = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.EST);
        var baseBody = ContentHelper.GetContentBaseValue(entity.Content.First(x => x.ContentType!.Name == ContentTypes.BODY));
        
        // Image Service
        var newBodyEn = _imageStorageService.ReplaceImages(bodyEn);
        var newBodyEt = _imageStorageService.ReplaceImages(bodyEt);
        var newBaseBody =  _imageStorageService.ReplaceImages(baseBody);
        
        // SET NEW VALUES
        ContentHelper.SetBaseLanguage(entity, ContentTypes.BODY, newBaseBody);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.EST, newBodyEt);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.ENG, newBodyEn);
        */
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