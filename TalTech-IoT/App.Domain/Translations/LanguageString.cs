using Base.Domain;

namespace App.Domain;

public class LanguageString : DomainEntityId
{
    public string Value { get; set; } = default!;
    
    // Might happen that user does not add a translation.
    public ICollection<LanguageStringTranslation>? LanguageStringTranslations { get; set; }
}