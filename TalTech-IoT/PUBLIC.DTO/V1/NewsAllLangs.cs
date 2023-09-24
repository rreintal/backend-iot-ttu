namespace Public.DTO.V1;

public class NewsAllLangs
{
    public Guid Id { get; set; }
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;

    public string Author { get; set; } = default!;
    
    // This is thumbnail image, not image inside content!
    // TODO - kas image on optional või ei?
    public string? Image { get; set; }
    public DateTime? CreatedAt { get; set; }

    // TODO- SIIN VÕIKSID OLLA AINULT ID + NAME, ET USERILE NÄIDATA

    public List<Public.DTO.V1.GetTopicArea> TopicAreas { get; set; } = default!;
}