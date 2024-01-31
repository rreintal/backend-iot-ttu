namespace Public.DTO.V1.Mappers;
/*
public class TEST<S, CT, R, Id> // Source, ContentTypes, Result, Id (PRIMARY KEY)
{
    public R CreateLanguageStringsWithAllCultures()
    {
        Id id;
        R result;
        
        // 
        
        return R;
    }
}
/*
 var newsId = Guid.NewGuid();
        var estTitle = postNews.Title.First(x => x.Culture == LanguageCulture.EST);
        var estBody = postNews.Body.First(x => x.Culture == LanguageCulture.EST);
        
        var bodyContentType = contentTypes.Where(x => x.Name == "BODY").First();
        var titleContentType = contentTypes.Where(x => x.Name == "TITLE").First();

        var bodyLangStrId = Guid.NewGuid();
        var bodyLangStr = new BLL.DTO.V1.LanguageString()
        {
            Id = bodyLangStrId,
            Value = estBody.Value
        };
        var bodyContent = new BLL.DTO.V1.Content()
        {
            NewsId = newsId,
            ContentTypeId = bodyContentType.Id,
            ContentType = bodyContentType,
            LanguageStringId = bodyLangStrId,
            LanguageString = bodyLangStr
        };

        bodyLangStr.Content = bodyContent;
        
        var titleLangStrId = Guid.NewGuid();
        var titleLangStr = new BLL.DTO.V1.LanguageString()
        {
            Id = titleLangStrId,
            Value = estTitle.Value
        };

        var titleContent = new BLL.DTO.V1.Content()
        {
            NewsId = newsId,
            ContentTypeId = titleContentType.Id,
            ContentType = titleContentType,
            LanguageStringId = titleLangStrId,
            LanguageString = titleLangStr
        };
        titleLangStr.Content = titleContent;

        var bodyTranslations = new List<BLL.DTO.V1.LanguageStringTranslation>();
        foreach (var bodyDto in postNews.Body)
        {
            var langStr = new BLL.DTO.V1.LanguageStringTranslation()
            {
                LanguageCulture = bodyDto.Culture,
                TranslationValue = bodyDto.Value,
                LanguageStringId = bodyLangStr.Id
            };
            bodyTranslations.Add(langStr);
        }
        var titleTranslations = new List<BLL.DTO.V1.LanguageStringTranslation>();
        
        foreach (var titleDto in postNews.Title)
        {
            var langStr = new BLL.DTO.V1.LanguageStringTranslation()
            {
                LanguageCulture = titleDto.Culture,
                TranslationValue = titleDto.Value,
                LanguageStringId = titleLangStr.Id
            };
            titleTranslations.Add(langStr);
        }

        titleLangStr.LanguageStringTranslations = titleTranslations;
        bodyLangStr.LanguageStringTranslations = bodyTranslations;

        var res = new BLL.DTO.V1.News()
        {
            Content = new List<BLL.DTO.V1.Content>()
            { 
                titleContent, bodyContent
            },
            Author = postNews.Author
        };

        res.TopicAreas = TopicAreaMapper.Map(postNews.TopicAreas);
        res.Image = postNews.Image;

        return res;
*/