using App.Domain;
using BLL.DTO.V1;

namespace Public.DTO.V1.Mappers;

public class GetPageContentMapper
{
    public static PageContent Map(App.Domain.PageContent domainObject)
    {
        return new PageContent()
        {
            PageIdentifier = domainObject.PageIdentifier,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = domainObject.GetContentValue(ContentTypes.BODY, lang),
                    Culture = lang
                };
            }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = domainObject.GetContentValue(ContentTypes.TITLE, lang),
                    Culture = lang
                };
            }).ToList(),
        };
        
    }
}