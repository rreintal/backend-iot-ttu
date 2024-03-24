namespace Public.DTO.V1;

public class TopicAreaWithCount
{
    // Id of TopicArea
    public Guid Id { get; set; }

    // Name of the TopicArea
    public string Name { get; set; } = default!;
    
    // Count with how many News its associated!
    public int Count { get; set; } = default!;
}