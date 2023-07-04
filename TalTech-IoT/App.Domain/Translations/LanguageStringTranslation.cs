using App.Domain.Translations;
using Base.Domain;

namespace App.Domain;

public class LanguageStringTranslation : DomainEntityId
{
    public Guid LanguageStringId { get; set; }
    public LanguageString? LanguageString { get; set; }
    
    public string LanguageCulture { get; set; } = default!;
    public string TranslationValue { get; set; } = default!;
}