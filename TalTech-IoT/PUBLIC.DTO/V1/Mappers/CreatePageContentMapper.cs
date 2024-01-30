using App.Domain;
using App.Domain.Translations;

namespace Public.DTO.V1.Mappers;

public class CreatePageContentMapper
{
    public static App.Domain.PageContent Map(PageContent entity, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        // title
        var pageContentId = Guid.NewGuid();
        var estTitle = entity.Title.First(x => x.Culture == LanguageCulture.EST);
        var estBody = entity.Body.First(x => x.Culture == LanguageCulture.EST);
        
        var bct = contentTypes.First(x => x.Name == "BODY");
        var tcp = contentTypes.First(x => x.Name == "TITLE");
        var bodyContentType = new ContentType()
        {
            Id = bct.Id,
            Name = bct.Name
        };

        var titleContentType = new ContentType()
        {
            Id = tcp.Id,
            Name = tcp.Name
        };
        
        
        
        var bodyLangStrId = Guid.NewGuid();
        var bodyLangStr = new LanguageString()
        {
            Id = bodyLangStrId,
            Value = estBody.Value
        };
        var bodyContent = new App.Domain.Content()
        {
            
            ContentTypeId = bodyContentType.Id,
            LanguageStringId = bodyLangStrId,
            LanguageString = bodyLangStr
        };

        bodyLangStr.Content = bodyContent;
        
        var titleLangStrId = Guid.NewGuid();
        var titleLangStr = new LanguageString()
        {
            Id = titleLangStrId,
            Value = estTitle.Value
        };

        var titleContent = new Content()
        {
            PageContentId = pageContentId,
            ContentTypeId = titleContentType.Id,
            LanguageStringId = titleLangStrId,
            LanguageString = titleLangStr
        };
        titleLangStr.Content = titleContent;

        var bodyTranslations = new List<LanguageStringTranslation>();
        foreach (var bodyDto in entity.Body)
        {
            var langStr = new LanguageStringTranslation()
            {
                LanguageCulture = bodyDto.Culture,
                TranslationValue = bodyDto.Value,
                LanguageStringId = bodyLangStr.Id
            };
            bodyTranslations.Add(langStr);
        }
        var titleTranslations = new List<LanguageStringTranslation>();
        
        foreach (var titleDto in entity.Title)
        {
            var langStr = new LanguageStringTranslation()
            {
                LanguageCulture = titleDto.Culture,
                TranslationValue = titleDto.Value,
                LanguageStringId = titleLangStr.Id
            };
            titleTranslations.Add(langStr);
        }

        titleLangStr.LanguageStringTranslations = titleTranslations;
        bodyLangStr.LanguageStringTranslations = bodyTranslations;

        var res = new App.Domain.PageContent()
        {
            Content = new List<Content>()
            { 
                titleContent, bodyContent
            },
            PageIdentifier = entity.PageIdentifier
        };
        return res;
    }
}