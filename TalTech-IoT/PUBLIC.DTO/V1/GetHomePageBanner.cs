namespace Public.DTO.V1;

public class GetHomePageBanner
{
    public Guid Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Image { get; set; } = default!;
}