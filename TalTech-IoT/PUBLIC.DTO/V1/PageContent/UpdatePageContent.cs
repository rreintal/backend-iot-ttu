namespace Public.DTO.V1;

public class UpdatePageContent
{
    public List<ContentDto> Body { get; set; } = default!;

    public List<ContentDto> Title { get; set; } = default!;
}