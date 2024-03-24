using App.Domain.Translations;
using Base.Domain;

namespace App.Domain;

public class TopicArea : DomainEntityId
{
    public Guid LanguageStringId { get; set; }
    public LanguageString? LanguageString { get; set; }

    public string GetName()
    {
        return LanguageString!.LanguageStringTranslations.First().TranslationValue;
    }
}