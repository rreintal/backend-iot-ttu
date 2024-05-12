using App.Domain;
using BLL.DTO.V1;
using LanguageStringTranslation = BLL.DTO.V1.LanguageStringTranslation;

namespace Public.DTO.V1.Mappers;

public static class CreateTopicAreaMapper
{
    public static BLL.DTO.V1.TopicArea Map(PostTopicAreaDto data)
    {
        var et = data.Name.First(x => x.Culture == LanguageCulture.EST);
        var en = data.Name.First(x => x.Culture == LanguageCulture.ENG);

        var topicAreaId = Guid.NewGuid();
        var languageStringId = Guid.NewGuid();
        
        var languageString = new LanguageString()
        {
            Id = languageStringId,
            TopicAreaId = topicAreaId,
            Value = et.Value
        };

        var etTranslation = new BLL.DTO.V1.LanguageStringTranslation()
        {
            TranslationValue = et.Value,
            LanguageCulture = et.Culture,
            LanguageStringId = languageStringId
        };

        var enTranslation = new BLL.DTO.V1.LanguageStringTranslation()
        {
            TranslationValue = en.Value,
            LanguageCulture = en.Culture,
            LanguageStringId = languageStringId
        };

        languageString.LanguageStringTranslations = new List<LanguageStringTranslation>()
        {
            etTranslation, enTranslation
        };

        var result = new BLL.DTO.V1.TopicArea()
        {
            Id = topicAreaId,
            LanguageString = languageString,
            LanguageStringId = languageStringId
        };
        

        return result;
    }
}