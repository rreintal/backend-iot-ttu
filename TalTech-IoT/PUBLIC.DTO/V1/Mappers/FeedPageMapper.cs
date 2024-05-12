using System.Security.Cryptography.X509Certificates;
using BLL.DTO.V1;
using Public.DTO.V1.FeedPage;
using FeedPageCategory = BLL.DTO.V1.FeedPageCategory;

namespace Public.DTO.V1.Mappers;

public class FeedPageMapper
{
    public static BLL.DTO.V1.FeedPage Map(Public.DTO.V1.FeedPage.FeedPage entity)
    {
        return new BLL.DTO.V1.FeedPage() {
            FeedPageName = entity.FeedPageName,
        };
    }


    public static Public.DTO.V1.FeedPage.FeedPage Map(BLL.DTO.V1.FeedPage entity)
    {
        return new Public.DTO.V1.FeedPage.FeedPage()
        {
            Id = entity.Id,
            FeedPageName = entity.FeedPageName, 
            FeedPageCategories = entity.FeedPageCategories.Select(e =>
            {
                return FeedPageCategoryMapper.Map(e);
            }).ToList()
        };
    }

    public static GetFeedPageTranslated MapTranslated(BLL.DTO.V1.FeedPage entity, string language)
    {
        return new GetFeedPageTranslated()
        {
            Id = entity.Id,
            FeedPageName = entity.FeedPageName,
            FeedPageCategories = entity.FeedPageCategories.Select(e => FeedPageCategoryMapper.MapTranslated(e, language)).ToList()
        };
    }
    
    public static Public.DTO.V1.FeedPage.FeedPage Map(BLL.DTO.V1.FeedPage entity, string language)
    {
        return new Public.DTO.V1.FeedPage.FeedPage()
        {
            Id = entity.Id,
            FeedPageName = entity.FeedPageName, 
            FeedPageCategories = entity.FeedPageCategories.Select(e =>
            {
                return FeedPageCategoryMapper.Map(e, language);
            }).ToList()
        };
    }
}