using Base.Domain;

namespace DAL.DTO.V1;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicAreaId { get; set; }
    public TopicArea? ParentTopicArea { get; set; }
    
    public string Name { get; set; } = default!;
}