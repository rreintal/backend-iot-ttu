namespace Public.DTO.V1;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;

    public string Author { get; set; } = default!;
    
    // This is thumbnail image, not image inside content!
    // TODO - kas image on optional või ei?
    public string? Image { get; set; }
    public DateTime? CreatedAt { get; set; }

    public List<Public.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

}
