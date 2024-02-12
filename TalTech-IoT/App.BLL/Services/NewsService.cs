using System.Text.RegularExpressions;
using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Update;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using App.Domain.Constants;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using Public.DTO;
using Public.DTO.Content;
using Content = App.Domain.Content;
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
        
        var oldBodyEn = existingEntity.GetContentValue(ContentTypes.BODY, LanguageCulture.ENG);
        var oldBodyEt = existingEntity.GetContentValue(ContentTypes.BODY, LanguageCulture.EST);
        var oldBaseBody = existingEntity.GetBaseLanguageContent(ContentTypes.BODY);
        
        var bodyEn = ContentHelper.GetContentValue(entity, ContentTypes.BODY, LanguageCulture.ENG);
        var bodyEt = ContentHelper.GetContentValue(entity, ContentTypes.BODY, LanguageCulture.EST);
        var baseBody = ContentHelper.GetContentBaseValue(entity.Content.First(x => x.ContentType!.Name == ContentTypes.BODY));

        var updateData = new UpdateContent()
        {
            Items = new List<UpdateItem>()
        };

        var updateBodyEn = new UpdateItem()
        {
            OldContent = oldBodyEn,
            NewContent = bodyEn,
            Sequence = 0
        };

        var updateBodyEt = new UpdateItem()
        {
            OldContent = oldBodyEt,
            NewContent = bodyEt,
            Sequence = 1
        };

        /*
        var updatedBase = new UpdateItem()
        {
            OldContent = oldBaseBody,
            NewContent = baseBody,
            Sequence = 2
        };
        */

        updateData.Items.Add(updateBodyEn);
        updateData.Items.Add(updateBodyEt);
        //updateData.Items.Add(updatedBase);

        var updateResult = await _imageStorageService.Update(updateData);
        var a = 2;
        //ContentHelper.SetBaseLanguage(entity, ContentTypes.BODY, updateResult.First(e => e.Sequence == 2).NewContent);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.EST, updateResult.First(e => e.Sequence == 1).NewContent);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.ENG, updateResult.First(e => e.Sequence == 0).NewContent);

        /*
         Tee Map
        {1, oldBodyEn}
        {2, oldBodyEt}
        {3, oldBodyBase}
        
        // OLD VALUES
        
        
        // NEW VALUES
        

        // UPDATED VALUES
        var newBodyEn = _imageStorageService.Update(oldBodyEn, bodyEn);
        var newBodyEt = _imageStorageService.Update(oldBodyEt, bodyEt);
        var newBaseBody =  _imageStorageService.Update(oldBaseBody, baseBody);
        
        // SET NEW VALUES
        ContentHelper.SetBaseLanguage(entity, ContentTypes.BODY, newBaseBody);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.EST, newBodyEt);
        ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.ENG, newBodyEn);
        */
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var updatedDalEntity =  await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
        return bllEntity;
        
    }

    public async Task<News> AddAsync(News entity)
    {
        // TODO: thumbnail
        try
        {
            entity.ThumbnailImage = ThumbnailService.Compress(entity.Image);
        }
        catch (Exception e)
        {
            entity.ThumbnailImage = "IMAGE COMPRESSING THREW AND EXCEPTION!";
        }
        
        var data = new SaveContent()
        {
            Items = new List<SaveItem>()
        };
        var bodyEn = new SaveItem()
        {
            Sequence = 0,
            Content = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.ENG)
        };
        var bodyEt = new SaveItem()
        {
            Sequence = 1,
            Content = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.EST)
        };
        
        /*
        var bodyBaseContent = ContentHelper.GetContentBaseValue(entity.Content.First(x => x.ContentType!.Name == ContentTypes.BODY));
        var bodyBase = new SaveItem()
        {
            Sequence = 2,
            Content = bodyBaseContent
        };
        */
        
        data.Items.Add(bodyEn);
        data.Items.Add(bodyEt);
        //data.Items.Add(bodyBase);

        var cdnResult = await _imageStorageService.Save(data);

        var newBodyEn = cdnResult.FirstOrDefault(e => e.Sequence == 0)?.UpdatedContent;
        var newBodyEt = cdnResult.FirstOrDefault(e => e.Sequence == 1)?.UpdatedContent;
        //var newBBaseBody = cdnResult.FirstOrDefault(e => e.Sequence == 2)?.UpdatedContent;
        
        // check if content is not null
        if (newBodyEn != null)
        {
            ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.ENG, newBodyEn);    
        }

        if (newBodyEt != null)
        {
            //ContentHelper.SetBaseLanguage(entity, ContentTypes.BODY, newBBaseBody);
            ContentHelper.SetContentTranslationValue(entity, ContentTypes.BODY, LanguageCulture.EST, newBodyEt);    
        }
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var dalResult = await Uow.NewsRepository.AddAsync(dalEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
    }

    public async Task<News> RemoveAsync(News entity)
    {
        var data = new DeleteContent()
        {
            Items = new List<DeletePayloadContent>()
        };
        data.Items.Add(new DeletePayloadContent()
        {
            Content = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.ENG)
        });
        
        data.Items.Add(new DeletePayloadContent()
        {
            Content = entity.GetContentValue(ContentTypes.BODY, LanguageCulture.EST)
        });
        
        var bodyBaseContent = ContentHelper.GetContentBaseValue(entity.Content.First(x => x.ContentType!.Name == ContentTypes.BODY));
        data.Items.Add(new DeletePayloadContent()
        {
            Content = bodyBaseContent
        });

        var response = await _imageStorageService.Delete(data);
        
        // What to do if it fails?? notify user?
        if (response == false)
        {
            Console.WriteLine("NewsService: Delete to CDN failed!");            
        }

        // TODO: mõtle läbi. RemoveAsync peaks võtma ikka entity :)
        //var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
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