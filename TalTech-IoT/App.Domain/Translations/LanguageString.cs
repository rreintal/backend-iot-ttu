using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Translations;

public class LanguageString : DomainEntityId
{
    public ICollection<LanguageStringTranslation>? LanguageStringTranslations { get; set; }
    
    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public Guid? TopicAreaId { get; set; }
    public TopicArea? TopicArea { get; set; }
}