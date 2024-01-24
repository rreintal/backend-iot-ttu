using App.Domain;
using Base.Domain;

namespace BLL.DTO.V1;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicAreaId { get; set; }
    public TopicArea? ParentTopicArea { get; set; }
    
    public Guid? LanguageStringId { get; set; }
    public LanguageString? LanguageString { get; set; }
    
    
    public string GetName()
    {
        return LanguageString?.LanguageStringTranslations?.First().TranslationValue ?? "";
    }

    public string GetName(string languageCulture)
    {
        return LanguageString!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture).First()
            .TranslationValue;
    }

    public string GetCulture(string languageCulture)
    {
        
        return LanguageString!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture).First()
            .LanguageCulture;
    }
}