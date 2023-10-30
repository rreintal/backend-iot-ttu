using App.Domain;
using App.Domain.Translations;

namespace App.DAL.EF.Seeding;

public static class DomainFactory
{
    
    // MARK: Generic


    // MARK: Domain.News
    
    public static News CreateNews()
    {
        News news = new News()
        {
            Id = Guid.NewGuid(),
        };
        return news;
    }
    
    public static News SetAuthor(this News news, string Author)
    {
        news.Author = Author;
        return news;
    }

    public static News SetContent(this News news, Dictionary<string, string> contentMap, string contentType)
    {
        // Map<Content: LanguageCulture>
        var baselangStringId = Guid.NewGuid();
        var baseLanguageContent = contentMap[LanguageCulture.BASE_LANGUAGE];
        var baseLanguageString = new LanguageString()
        {
            Id = baselangStringId,
            Content = new Content()
            {
                ContentTypeId = AppDataSeeding.GetContentTypeId(contentType),
                NewsId = news.Id
            },
            Value = baseLanguageContent
        };

        List<LanguageStringTranslation> translations = new List<LanguageStringTranslation>();
        foreach (var (TranslationValue, LanguageCulture) in contentMap)
        {
            var translation = new LanguageStringTranslation()
            {
                Id = Guid.NewGuid(),
                LanguageCulture = LanguageCulture,
                TranslationValue = TranslationValue,
                LanguageStringId = baselangStringId
            };
            translations.Add(translation);
        }

        baseLanguageString.LanguageStringTranslations = translations;
        return news;
    }

    public static News SetTopicArea(this News news, TopicArea topicArea)
    {
        // TODO 
        var hta = new HasTopicArea()
        {
            Id = Guid.NewGuid(),
            TopicAreaId = topicArea.Id,
            NewsId = news.Id
        };

        if (news.HasTopicAreas == null)
        {
            news.HasTopicAreas = new List<HasTopicArea>() { hta };
        }
        else
        {
            news.HasTopicAreas.Add(hta);
        }
        return news;
    }
    
    // MARK : TopicArea

    public static TopicArea TopicArea()
    {
        return new TopicArea();
    }
    
    public static TopicArea SetValues(this TopicArea topicArea, string etValue, string enValue, Guid? id = null)
    {
        // Check if id is presented, if not then generate random
        Guid t1Id;
        if (id.HasValue)
        {
            t1Id = id.Value;
        }
        else
        {
            t1Id = Guid.NewGuid();
        }
        
        var t1StrId = Guid.NewGuid();
        topicArea.Id = t1Id;
        topicArea.LanguageStringId = t1StrId;
        
        var t1Str = new LanguageString()
        {
            Id = t1StrId,
            TopicAreaId = t1Id,
            Value = etValue
        };
        var t1Est = new LanguageStringTranslation()
        {
            LanguageStringId = t1StrId,
            LanguageCulture = LanguageCulture.EST,
            TranslationValue = etValue
        };
        var t1Eng = new LanguageStringTranslation()
        {
            LanguageStringId = t1StrId,
            LanguageCulture = LanguageCulture.ENG,
            TranslationValue = enValue
        };
        t1Str.LanguageStringTranslations = new List<LanguageStringTranslation>()
        {
            t1Est, t1Eng
        };

        topicArea.LanguageString = t1Str;
        return topicArea;
    }

    public static TopicArea SetParent(this TopicArea topicArea, TopicArea Parent)
    {
        topicArea.ParentTopicAreaId = Parent.Id;
        return topicArea;
    }
    
    
    
}