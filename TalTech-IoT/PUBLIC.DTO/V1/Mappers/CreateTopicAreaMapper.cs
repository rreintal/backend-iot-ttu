using App.Domain;
using AutoMapper;
using Base.DAL;
using BLL.DTO.V1;
using LanguageStringTranslation = BLL.DTO.V1.LanguageStringTranslation;

namespace Public.DTO.V1.Mappers;

public class CreateTopicAreaMapper : BaseMapper<Public.DTO.V1.PostTopicAreaDto, BLL.DTO.V1.TopicArea>
{
    public CreateTopicAreaMapper(IMapper mapper) : base(mapper)
    {
    }

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

        // If it has also Parent Topic
        if (data.ParentTopicId != null)
        {
            result.ParentTopicAreaId = data.ParentTopicId;
        }

        return result;
    }
}