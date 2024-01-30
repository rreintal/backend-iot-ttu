using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class ChangePasswordModel
{
    [Required]
    public string Password { get; set; } = default!;
}