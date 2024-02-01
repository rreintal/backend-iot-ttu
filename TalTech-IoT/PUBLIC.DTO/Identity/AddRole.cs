using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Public.DTO.Identity;

public class AddRole
{
    [Required]
    public Guid UserId { get; set; } = default!;
    
    [Required]
    public Guid RoleId { get; set; } = default!;
}