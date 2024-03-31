namespace Public.DTO.V1;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;

    public string Author { get; set; } = default!;
    public string? Image { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Public.DTO.V1.GetTopicArea> TopicAreas { get; set; } = default!;

    public int ViewCount { get; set; }

}
