using Base.Domain;
namespace Public.DTO.V1;

public class TopicArea : DomainEntityId
{
    public string Name { get; set; } = default!;
    public List<TopicArea>? ChildrenTopicAreas { get; set; }
    
}

