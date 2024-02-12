using App.Domain;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class GetFeedPageCategoryWithoutPostsMapper
{
    public static Public.DTO.V1.FeedPage.GetFeedPageCategoryWithoutPosts Map(BLL.DTO.V1.FeedPageCategory entity, string languageCulture)
    {
        return new Public.DTO.V1.FeedPage.GetFeedPageCategoryWithoutPosts()
        {
            // TODO: add post
            Id = entity.Id,
            Title = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture)
        };
    }
    
    /*
     *
     * public static Public.DTO.V1.FeedPage.FeedPageCategory Map(BLL.DTO.V1.FeedPageCategory entity, string language)
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
     */
}