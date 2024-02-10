using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Translations;

public class LanguageString : DomainEntityId
{
    public string Value { get; set; } = default!;
    
    // Might happen that user does not add a translation.
    // Or not??? TODO
    public ICollection<LanguageStringTranslation>? LanguageStringTranslations { get; set; }
    
    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public Guid? TopicAreaId { get; set; }
    public TopicArea? TopicArea { get; set; }
}