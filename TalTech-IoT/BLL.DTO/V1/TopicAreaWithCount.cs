using Base.Domain;

namespace BLL.DTO.V1;

public class TopicAreaWithCount : DomainEntityId
{
    // Name of the TopicArea
    public string Name { get; set; } = default!;
    
    // Count with how many Projects/News its associated!
    public int Count { get; set; } = default!;
}