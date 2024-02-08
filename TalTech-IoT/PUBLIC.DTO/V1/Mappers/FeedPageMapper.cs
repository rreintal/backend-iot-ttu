using System.Security.Cryptography.X509Certificates;
using BLL.DTO.V1;
using Public.DTO.V1.FeedPage;

namespace Public.DTO.V1.Mappers;

public class FeedPageMapper
{
    public static BLL.DTO.V1.FeedPage Map(Public.DTO.V1.FeedPage.FeedPage entity)
    {
        return new BLL.DTO.V1.FeedPage() {
            FeedPageName = entity.FeedPageName, // TODO: mapper for categories!!, if categories present!
        };
    }


    public static Public.DTO.V1.FeedPage.FeedPage Map(BLL.DTO.V1.FeedPage entity)
    {
        return new Public.DTO.V1.FeedPage.FeedPage()
        {
            Id = entity.Id,
            FeedPageName = entity.FeedPageName, // TODO: feedPage mapper
        };
    }
    
    /*
     *
     * public Guid FeedPageId { get; set; }
    public FeedPage? FeedPage { get; set; }
    
    public List<Content> Content { get; set; } = default!;
    public List<FeedPagePost>? FeedPagePosts { get; set; }
     */
}