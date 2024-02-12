using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.FeedPage;
using ContentType = BLL.DTO.V1.ContentType;
using FeedPageCategory = Public.DTO.V1.FeedPage.FeedPageCategory;

namespace Public.DTO.V1.Mappers;

public class PostFeedPageCategoryWithPageIdenitiferMapper
{
    public static BLL.DTO.V1.FeedPageCategory Map(PostFeedPageCategoryWithPageIdentifier entity, List<ContentType> contentTypes, Guid feedPageId)
    {
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, feedPageId,
            ContentHelper.EContentHelperEntityType.FeedPageCategory);
        return new BLL.DTO.V1.FeedPageCategory()
        {
            Content = new List<BLL.DTO.V1.Content>() { titleContent },
            FeedPageId = feedPageId
        };
    }
}