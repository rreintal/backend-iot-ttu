using Base.Domain;

namespace DAL.DTO.V1;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicAreaId { get; set; }
    public TopicArea? ParentTopicArea { get; set; }
    
    public Guid? LanguageStringId { get; set; }
    public LanguageString? LanguageString { get; set; }
}