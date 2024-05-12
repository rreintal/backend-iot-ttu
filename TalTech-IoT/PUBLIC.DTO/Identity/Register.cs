using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

// TODO: constraints for variables
public class Register
{
    [MinLength(5)]
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    [MinLength(2)]
    [MaxLength(64)]
    public string Firstname { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(64)]
    public string Lastname { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(64)]
    public string Username { get; set; } = default!;
    
    [MinLength(8)]
    [MaxLength(64)]
    public string Password { get; set; } = default!;

    public Guid RoleId { get; set; } = default!;
}