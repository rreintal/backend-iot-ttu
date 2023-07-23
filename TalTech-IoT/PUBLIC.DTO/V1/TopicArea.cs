using Base.Domain;
namespace Public.DTO.V1;

public class TopicArea : DomainEntityId
{
    public Guid? ParentTopicId { get; set; }
    public string Name { get; set; } = default!;
    
}

