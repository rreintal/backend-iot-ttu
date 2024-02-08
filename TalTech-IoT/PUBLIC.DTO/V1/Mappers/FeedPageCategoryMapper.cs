using App.Domain;
using Public.DTO.Content;
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
}