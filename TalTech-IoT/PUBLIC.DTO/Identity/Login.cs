using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class Login
{
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    [MaxLength(128)]
    public string Password { get; set; } = default!;
}