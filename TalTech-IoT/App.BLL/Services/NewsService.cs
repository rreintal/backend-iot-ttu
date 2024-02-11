using System.Text.RegularExpressions;
using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using App.Domain.Constants;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using Public.DTO;
using Public.DTO.Content;
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
        var titleContentType = Uow.ContentTypesRepository.FindByName(ContentTypes.TITLE);
        var bodyContentType = Uow.ContentTypesRepository.FindByName(ContentTypes.BODY);
        
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
    

    public async Task<IEnumerable<News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageString)
    {
        // TODO: add this method to common interface w service/repo
        return (await Uow.NewsRepository.AllAsyncFiltered(filterSet, languageString)).Select(e => _mapper.Map<News>(e));
    }
    

    public async Task<News?> UpdateNews(UpdateNews entity)
    {
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateNews>(entity);
        var updatedDalEntity =  await Uow.NewsRepository.Update(dalEntity);
        var bllEntity = _mapper.Map<News?>(updatedDalEntity);
        return bllEntity;
        
    }

    public async Task<News?> FindByIdAllTranslationsAsync(Guid id)
    {
         var entity = await Uow.NewsRepository.FindByIdWithAllTranslationsAsync(id);
         return _mapper.Map<News>(entity);
    }

    public override News Add(News entity)
    {
        // Add thumbnail
        try
        {
            entity.ThumbnailImage = ThumbnailService.Compress(entity.Image);
        }
        catch (Exception e)
        {
            entity.ThumbnailImage = "IMAGE COMPRESSING THREW AND EXCEPTION!";
        }
        /*
        // Testing CDN
        var estonianBody = domainObject.GetContentValue(ContentTypes.BODY, LanguageCulture.EST);
        estonianBody = await HandleImageContent(estonianBody);
        var englishBody = domainObject.GetContentValue(ContentTypes.BODY, LanguageCulture.ENG);
        englishBody = await HandleImageContent(englishBody);
        
        // Set the modified content!
        domainObject.SetContentTranslationValue(ContentTypes.BODY, LanguageCulture.EST, estonianBody);
        domainObject.SetContentTranslationValue(ContentTypes.BODY, LanguageCulture.ENG, englishBody);
        // -----
        */
        
        
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.News>(entity);
        var dalResult = Uow.NewsRepository.Add(dalEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
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
    
    // TODO: maybe this inside ImageStorageService as Save?
    /*
    private async Task<string> HandleImageContent(string content)
    {
        var srcElementRegex = "<img[^>]*>";
        List<string> srcTagsList = new List<string> {};
        
        MatchCollection matches = Regex.Matches(content, srcElementRegex);

        foreach (Match match in matches)
        {
            if (match.Success)
            {
                srcTagsList.Add(match.Value);
            }
        }

        var imageServicePayload = new SaveImagesDTO()
        {
            data = new List<SaveImageDTO>()
        };
        
        string base64Regex = "<img src=\"data:image/(jpeg|png|jpg|webp);base64,([^\"]+)\"";

        foreach (var tag in srcTagsList)
        {
            Match match = Regex.Match(tag, base64Regex);
            if (match.Success)
            {
                var dto = new SaveImageDTO()
                {
                    // march.Groups[1] for the file format (jpeg/png ... )
                    // match.Groups[2].Value to get the base64 data
                    FileFormat = match.Groups[1].Value,
                    ImageContent = match.Groups[2].Value, 
                    Sequence = srcTagsList.IndexOf(tag) // TODO: if there are multiple same images, make some buffer list
                };
                imageServicePayload.data.Add(dto);
            }
        }
        
        var saveImagesResultsDto = await _imageStorageService.Save(imageServicePayload);

        foreach (var resultDto in saveImagesResultsDto!.Results)
        {
            var baseLoc = ImageStorageServiceConstants.IMAGE_PUBLIC_LOCATION;
            var itemFromOriginalArray = srcTagsList[resultDto.Sequence];
            var newItem = $"<img src=\"{baseLoc}{resultDto.Link}.png\">"; // TODO: remove this .png here
            content = content.Replace(itemFromOriginalArray, newItem);
        }
        return content;
    }
    */

    
}