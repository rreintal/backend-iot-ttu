using Base.Domain;

namespace Public.DTO.V1;

public class TopicAreaWithTranslation : DomainEntityId
{
    public List<ContentDto> Content { get; set; } = default!;
    public List<TopicAreaWithTranslation>? ChildrenTopicAreas { get; set; }
    
    
}