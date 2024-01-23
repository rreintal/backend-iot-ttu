namespace Public.DTO.Identity;

// TODO: constraints for variables
public class Register
{
    public string Email { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}