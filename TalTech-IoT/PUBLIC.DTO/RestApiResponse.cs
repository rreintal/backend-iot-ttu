using System.Net;
namespace Public.DTO;

public class RestApiResponse
{
    public string Message { get; set; } = default!;
    public HttpStatusCode Status { get; set; }
}