using System.Net;

namespace App.Domain;

public class RestApiErrorResponse
{
    public string Message { get; set; } = default!;
    public HttpStatusCode StatusCode { get; set; }
}