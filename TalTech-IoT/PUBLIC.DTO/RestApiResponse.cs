namespace Public.DTO;

public class RestApiResponse
{
    public string Message { get; set; } = default!;
    public int StatusCode { get; set; } = default!;
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}