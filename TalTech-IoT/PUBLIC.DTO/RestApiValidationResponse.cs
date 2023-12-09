using System.Net;

namespace Public.DTO;

public class RestApiValidationResponse
{
    public HttpStatusCode Code { get; set; }
    public List<ValidationError> ValidationErrors { get; set; } = default!;
}