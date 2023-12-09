using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class ContactForm
{
    [Required]
    // TODO: minlenght?
    [MaxLength(64)]
    public string Email { get; set; } = default!;
    
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;
    
    [Required]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;
    
    public string? Phone { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string MessageText { get; set; } = default!;
}