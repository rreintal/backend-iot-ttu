
using System.Security.Cryptography.X509Certificates;
using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.FeedPage;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class FeedPagePostMapper
{
    public static BLL.DTO.V1.FeedPagePost Map(Public.DTO.V1.FeedPage.FeedPagePost entity, List<ContentType> contentTypes)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var postId = Guid.NewGuid();
        var content = ContentHelper.CreateContent(entity.Content, titleContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);
        return new BLL.DTO.V1.FeedPagePost()
        {
            Id = postId,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            Content = new List<BLL.DTO.V1.Content>()
            {
                content
            },
        };
    }

    public static BLL.DTO.V1.FeedPagePost MapForUpdate(Public.DTO.V1.FeedPage.FeedPagePost entity, List<ContentType> contentTypes, Guid postId)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var content = ContentHelper.CreateContent(entity.Content, titleContentType, postId,
            ContentHelper.EContentHelperEntityType.FeedPagePost);
        return new BLL.DTO.V1.FeedPagePost()
        {
            Id = postId,
            FeedPageCategoryId = entity.FeedPageCategoryId,
            Content = new List<BLL.DTO.V1.Content>()
            {
                content
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
            Content = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Culture = lang,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, lang)
                };
            }).ToList()
        };
    }
}