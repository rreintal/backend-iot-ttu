using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class ContactForm
{
    [Required]
    public string Email { get; set; } = default!;
    
    [Required]
    public string FirstName { get; set; } = default!;
    
    [Required]
    public string LastName { get; set; } = default!;
    
    public string? Phone { get; set; }
    
    // should it be also translated?!
    public string MessageText { get; set; } = default!;
}