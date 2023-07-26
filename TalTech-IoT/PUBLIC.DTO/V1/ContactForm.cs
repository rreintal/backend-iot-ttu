namespace Public.DTO.V1;

public class ContactForm
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    
    public string? Phone { get; set; }
    
    // should it be also translated?!
    public string MessageText { get; set; } = default!;
}