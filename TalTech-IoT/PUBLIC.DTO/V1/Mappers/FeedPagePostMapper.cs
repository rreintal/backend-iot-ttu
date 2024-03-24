
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.FeedPage;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class FeedPagePostMapper
{

    public static Public.DTO.V1.FeedPage.GetFeedPageCategoryPostTranslated MapTranslated(BLL.DTO.V1.FeedPagePost entity, string languageCulture)
    {
        return new GetFeedPageCategoryPostTranslated()
        {
            Id = entity.Id,
            Body = ContentHelper.GetContentValue(entity, ContentTypes.BODY, languageCulture),
            Title = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture),
            FeedPageCategoryId = entity.FeedPageCategoryId,
            CreatedAt = entity.CreatedAt
        };
    }
    public static BLL.DTO.V1.FeedPagePost Map(Public.DTO.V1.FeedPage.FeedPagePost entity, List<ContentType> contentTypes)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var postId = Guid.NewGuid();
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);
        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);

        var date = (DateTime)entity.CreatedAt;
        
        return new BLL.DTO.V1.FeedPagePost()
        {
            Id = postId,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            Content = new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            }
        };
    }

    public static BLL.DTO.V1.FeedPagePost MapForUpdate(Public.DTO.V1.FeedPage.FeedPagePost entity, List<ContentType> contentTypes, Guid postId)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);
        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);
        return new BLL.DTO.V1.FeedPagePost()
        {
            Id = postId,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            Content = new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            },
        };
    }

    public static Public.DTO.V1.FeedPage.FeedPagePost Map(BLL.DTO.V1.FeedPagePost entity)
    {
        // ALL LANGS
        
        return new Public.DTO.V1.FeedPage.FeedPagePost()
        {
            Id = entity.Id,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            CreatedAt = entity.CreatedAt,
            Title = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture)
                };
            }).ToList(),
            Body = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.BODY, languageCulture)// entity.GetContentValue(ContentTypes.BODY, languageCulture)
                };
            }).ToList()
        };
    }
    
    public static Public.DTO.V1.FeedPage.FeedPagePost Map(BLL.DTO.V1.FeedPagePost entity, string language)
    {
        // ALL LANGS
        
        return new Public.DTO.V1.FeedPage.FeedPagePost()
        {
            Id = entity.Id,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            CreatedAt = entity.CreatedAt,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = language,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, language)
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = language,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.BODY, language)
                }
            }
        };
    }
}