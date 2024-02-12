using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.FeedPage;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class FeedPageCategoryMapper
{
    public static BLL.DTO.V1.FeedPageCategory Map(Public.DTO.V1.FeedPage.FeedPageCategory entity, List<ContentType> contentTypes)
    {
        var feedPageCategoryId = Guid.NewGuid();
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, feedPageCategoryId,
            ContentHelper.EContentHelperEntityType.FeedPageCategory);
        return new BLL.DTO.V1.FeedPageCategory()
        {
            Content = new List<BLL.DTO.V1.Content>() { titleContent },
            FeedPageId = entity.FeedPageId
            // TODO: add posts!
        };
    }
    public static BLL.DTO.V1.FeedPageCategory MapToUpdate(Public.DTO.V1.FeedPage.FeedPageCategory entity, List<ContentType> contentTypes, Guid id)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, id,
            ContentHelper.EContentHelperEntityType.FeedPageCategory);
        return new BLL.DTO.V1.FeedPageCategory()
        {
            Id = id,
            Content = new List<BLL.DTO.V1.Content>() { titleContent },
            FeedPageId = entity.FeedPageId
            // TODO: add posts!
        };
    }

    public static Public.DTO.V1.FeedPage.GetFeedPageCategoryTranslated MapTranslated(BLL.DTO.V1.FeedPageCategory entity,
        string languageCulture)
    {
        return new GetFeedPageCategoryTranslated()
        {
            Id = entity.Id,
            Title = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture),
            FeedPageCategoryPost = entity.FeedPagePosts
                .Select(e => FeedPagePostMapper.MapTranslated(e, languageCulture)).ToList()
        };
    }

    public static Public.DTO.V1.FeedPage.FeedPageCategory Map(BLL.DTO.V1.FeedPageCategory entity)
    {
        // ALL LANGS
        return new Public.DTO.V1.FeedPage.FeedPageCategory()
        {
            // TODO: add post
            Id = entity.Id,
            FeedPageId = entity.FeedPageId,
            Title = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Culture = lang,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, lang)
                };
            }).ToList(),
            FeedPageCategoryPosts = entity.FeedPagePosts.Select(e => FeedPagePostMapper.Map(e)).ToList()
        };
    }
    
    public static Public.DTO.V1.FeedPage.FeedPageCategory Map(BLL.DTO.V1.FeedPageCategory entity, string language)
    {
        return new Public.DTO.V1.FeedPage.FeedPageCategory()
        {
            // TODO: add post
            Id = entity.Id,
            FeedPageId = entity.FeedPageId,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = language,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, language)
                }
            },
            FeedPageCategoryPosts = entity.FeedPagePosts.Select(e => FeedPagePostMapper.Map(e, language)).ToList()
        };
    }
}
