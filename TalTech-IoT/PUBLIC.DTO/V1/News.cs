namespace Public.DTO.V1;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public Byte[]? Image { get; set; }
    public DateTime? CreatedAt { get; set; }

}