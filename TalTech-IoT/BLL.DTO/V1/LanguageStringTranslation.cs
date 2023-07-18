using Base.Domain;

namespace BLL.DTO.V1;

public class LanguageStringTranslation : DomainEntityId
{
    public Guid LanguageStringId { get; set; }
    public BLL.DTO.V1.LanguageString? LanguageString { get; set; }

    public string LanguageCulture { get; set; } = default!;
    public string TranslationValue { get; set; } = default!;
}