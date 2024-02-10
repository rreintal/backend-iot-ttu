using System.Reflection.Emit;
using App.Domain;
using Public.DTO.Content;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class HomePageBannerMapper
{
    public static BLL.DTO.V1.HomePageBanner Map(Public.DTO.V1.HomePageBanner data, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var entityId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(data.Title, titleContentType, entityId,
            ContentHelper.EContentHelperEntityType.HomePageBanner);
        
        var bodyContent = ContentHelper.CreateContent(data.Body, bodyContentType, entityId,
            ContentHelper.EContentHelperEntityType.HomePageBanner);

        var res = new BLL.DTO.V1.HomePageBanner()
        {
            Id = entityId,
            Content = new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            },
            Image = data.Image
        };
        return res;
    }

    public static DAL.DTO.V1.HomePageBannerSequence Map(Public.DTO.V1.HomePageBannerSequence data)
    {
        return new DAL.DTO.V1.HomePageBannerSequence()
        {
            HomePageBannerId = data.HomePageBannerId,
            SequenceNumber = data.SequenceNumber
        };
    }

    public static Public.DTO.V1.HomePageBanner Map(BLL.DTO.V1.HomePageBanner entity)
    {
        return new HomePageBanner()
        {
            Id = entity.Id,
            Image = entity.Image,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = entity.GetContentValue(ContentTypes.BODY, lang),
                    Culture = lang
                };
            }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = entity.GetContentValue(ContentTypes.TITLE, lang),
                    Culture = lang
                };
            }).ToList(),
            SequenceNumber = entity.SequenceNumber
        };
    }

    public static BLL.DTO.V1.HomePageBanner MapUpdate(Public.DTO.V1.HomePageBanner entity, List<ContentType> contentTypes)
    {
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, entity.Id,
            ContentHelper.EContentHelperEntityType.HomePageBanner);
        
        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, entity.Id,
            ContentHelper.EContentHelperEntityType.HomePageBanner);
        return new BLL.DTO.V1.HomePageBanner()
        {
            Id = entity.Id,
            Image = entity.Image,
            Content = new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            }
        };
    }
}