namespace Public.DTO.V1;

public class GetPageContent
{
    public string PageIdentifier { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Title { get; set; } = default!;
}