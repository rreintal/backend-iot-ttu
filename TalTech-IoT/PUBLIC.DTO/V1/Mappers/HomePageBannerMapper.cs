using App.Domain;
using Public.DTO.Content;

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

    public static BLL.DTO.V1.UpdateHomePageBanner Map(Public.DTO.V1.UpdateHomePageBanner entity)
    {
        var bllEntity =  new BLL.DTO.V1.UpdateHomePageBanner()
        {
          Id  = entity.Id,
          Body =
              entity.Body.Select(publicDto =>
              {
                  return new BLL.DTO.V1.ContentDto()
                  {
                      Culture = publicDto.Culture,
                      Value = publicDto.Value
                  };
              }).ToList(),
          Title = entity.Title.Select(publicDto =>
          {
              return new BLL.DTO.V1.ContentDto()
              {
                  Value = publicDto.Value,
                  Culture = publicDto.Culture
              };
          }).ToList(),
        };

        if (entity.Image != null)
        {
            bllEntity.Image = entity.Image;
        }

        return bllEntity;
    }

    public static Public.DTO.V1.UpdateHomePageBanner Map(BLL.DTO.V1.UpdateHomePageBanner entity)
    {
        return new UpdateHomePageBanner()
        {
            Id = entity.Id,
            Image = entity.Image,
            Body = entity.Body.Select(publicDto =>
            {
                return new Public.DTO.V1.ContentDto()
                {
                    Culture = publicDto.Culture,
                    Value = publicDto.Value
                };
            }).ToList(),
            Title = entity.Title.Select(publicDto =>
            {
                return new Public.DTO.V1.ContentDto()
                {
                    Culture = publicDto.Culture,
                    Value = publicDto.Value
                };
            }).ToList(),
            
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
        };
    }
}