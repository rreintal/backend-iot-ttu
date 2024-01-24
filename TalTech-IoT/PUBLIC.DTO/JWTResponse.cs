namespace Public.DTO;

public class JWTResponse
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string AppUserId { get; set; } = default!;

    public string Username { get; set; } = default!;
}