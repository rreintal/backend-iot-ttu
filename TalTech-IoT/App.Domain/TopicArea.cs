using App.Domain.Translations;
using Base.Domain;
using Contracts;

namespace App.Domain;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicAreaId { get; set; }
    public TopicArea? ParentTopicArea { get; set; }
    
    public Guid LanguageStringId { get; set; }
    public LanguageString LanguageString { get; set; } = default!;
}