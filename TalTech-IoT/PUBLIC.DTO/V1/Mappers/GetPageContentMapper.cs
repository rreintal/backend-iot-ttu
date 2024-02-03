using App.Domain;
using BLL.DTO.V1;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class GetPageContentMapper
{

    public static PageContent MapBLL(BLL.DTO.V1.PageContent entity)
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
    
    public static Public.DTO.V1.GetPageContent MapTemporaryHack(App.Domain.PageContent domainObject, string languageCulture)
    {
        return new GetPageContent()
        {
            PageIdentifier = domainObject.PageIdentifier,
            Body = domainObject.GetContentValue(ContentTypes.BODY, languageCulture),
            Title = domainObject.GetContentValue(ContentTypes.TITLE, languageCulture)
        };
    }
}