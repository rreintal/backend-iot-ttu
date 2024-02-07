using App.Domain;

namespace Public.DTO.V1.Mappers;

public class GetHomePageBannerMapper
{
    public static Public.DTO.V1.GetHomePageBanner Map(BLL.DTO.V1.HomePageBanner entity)
    {
        var result = new GetHomePageBanner()
        {
            Id = entity.Id,
            Body = entity.GetContentValue(ContentTypes.BODY),
            Title = entity.GetContentValue(ContentTypes.TITLE),
            Image = entity.Image
        };

        return result;
    }
}