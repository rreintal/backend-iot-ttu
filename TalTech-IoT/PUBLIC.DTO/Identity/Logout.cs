using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class Logout
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}