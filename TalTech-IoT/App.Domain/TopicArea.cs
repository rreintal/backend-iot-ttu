using App.Domain.Translations;
using Base.Domain;

namespace App.Domain;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicAreaId { get; set; }
    public TopicArea? ParentTopicArea { get; set; }
    
    public Guid LanguageStringId { get; set; }
    public LanguageString? LanguageString { get; set; }

    public string GetName()
    {
        return LanguageString!.LanguageStringTranslations.First().TranslationValue;
    }

    public bool HasParentTopicArea()
    {
        return ParentTopicArea != null;
    }


    public static TopicArea TopicAreaFactory(string etValue, string enValue)
    {
        var t1Id = Guid.NewGuid();
        var t1StrId = Guid.NewGuid();
        var t1 = new TopicArea()
        {
            Id = t1Id,
            LanguageStringId = t1StrId,

        };
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

        t1.LanguageString = t1Str;
        return t1;
    }
}