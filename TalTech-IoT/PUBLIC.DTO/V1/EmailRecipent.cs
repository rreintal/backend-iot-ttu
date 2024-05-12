using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class EmailRecipent
{
    public Guid? Id { get; set; }
    
    [MinLength(2)]
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    [MinLength(2)]
    [MaxLength(64)]
    public string? FirstName { get; set; }
    [MinLength(2)]
    [MaxLength(64)]
    public string? LastName { get; set; }
    [MinLength(2)]
    [MaxLength(2000)]
    public string? Comment { get; set; }
}