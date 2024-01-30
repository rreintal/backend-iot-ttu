using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class AddRole
{
    [Required]
    public Guid UserId { get; set; } = default!;
    [Required]
    public string Role { get; set; } = default!;
}