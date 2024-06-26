using App.Domain;
using App.Domain.Translations;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class CreatePageContentMapper
{
    public static BLL.DTO.V1.PageContent Map(PageContent entity, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var pageContentId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, pageContentId,
            ContentHelper.EContentHelperEntityType.PageContent);

        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, pageContentId,
            ContentHelper.EContentHelperEntityType.PageContent);
        

        var res = new BLL.DTO.V1.PageContent()
        {
            Content = new List<BLL.DTO.V1.Content>()
            { 
                titleContent, bodyContent
            },
            PageIdentifier = entity.PageIdentifier
        };
        return res;
    }
    
    
    
    // TODO: eemalda kui PageContent update on korras!
    public static App.Domain.PageContent MapHack(PageContent entity, List<BLL.DTO.V1.ContentType> contentTypes)
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
        };

        var titleContent = new App.Domain.Content()
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
            Content = new List<App.Domain.Content>()
            { 
                titleContent, bodyContent
            },
            PageIdentifier = entity.PageIdentifier
        };
        return res;
    }
}