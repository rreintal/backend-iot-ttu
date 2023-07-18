using Base.Domain;

namespace BLL.DTO.V1;

public class LanguageString : DomainEntityId
{
    public string Value { get; set; } = default!;
    
    public List<BLL.DTO.V1.LanguageStringTranslation>? LanguageStringTranslations { get; set; }

    // LanguageString can be a PageContent thing also!
    public Guid? ContentId { get; set; }
    public BLL.DTO.V1.Content? Content { get; set; }
}