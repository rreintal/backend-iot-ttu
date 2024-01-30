namespace Public.DTO;

public class NewsFilterSet
{
    public int? Size { get; set; }
    public int? Page { get; set; }
    public Guid? TopicAreaId { get; set; }

    public bool? IncludeBody { get; set; }
}