using System.ComponentModel.DataAnnotations;

namespace Public.DTO.Identity;

public class RegisterUnknown
{
    
    // TODO: Tee mingi cosntant class kus on need limiidi olemas Username.length, Fn.length etc
    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public string Firstname { get; set; } = default!;
    
    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public string Lastname { get; set; } = default!;
    
    [Required]
    [MinLength(4)]
    [MaxLength(32)]
    public string Username { get; set; } = default!;
    
    [Required]
    [MinLength(3)] // https://stackoverflow.com/questions/1423195/what-is-the-actual-minimum-length-of-an-email-address-as-defined-by-the-ietf
    [MaxLength(64)]
    public string Email { get; set; } = default!;
    
    [MinLength(3)]
    [MaxLength(12)]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public Guid RoleId { get; set; } = default!;
}