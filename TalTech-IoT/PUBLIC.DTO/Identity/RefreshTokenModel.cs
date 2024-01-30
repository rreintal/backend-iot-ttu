using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class RefreshTokenModel
{
    [Required]
    public string Jwt { get; set; } = default!;
    [Required]
    public string RefreshToken { get; set; } = default!;
}