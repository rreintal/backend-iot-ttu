using App.Domain;
using BLL.DTO.V1;
using Public.DTO.Content;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class PageContentMapper
{

    public static PageContent Map(BLL.DTO.V1.PageContent entity)
    {
        return new PageContent()
        {
            PageIdentifier = entity.PageIdentifier,
            Body = LanguageCulture.ALL_LANGUAGES.Select(LanguageCulture =>
            {
                return new ContentDto()
                {
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.BODY, LanguageCulture),
                    Culture = LanguageCulture
                };
            }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(LanguageCulture =>
            {
                return new ContentDto()
                {
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, LanguageCulture),
                    Culture = LanguageCulture
                };
            }).ToList()
        };
    }

    public static Public.DTO.V1.GetPageContent MapTemporaryHack(BLL.DTO.V1.PageContent bllObject, string languageCulture)
    {
        return new GetPageContent()
        {
            PageIdentifier = bllObject.PageIdentifier,
            Body = ContentHelper.GetContentValue(bllObject, ContentTypes.BODY, languageCulture),
            Title = ContentHelper.GetContentValue(bllObject, ContentTypes.TITLE, languageCulture),
        };
    }
}