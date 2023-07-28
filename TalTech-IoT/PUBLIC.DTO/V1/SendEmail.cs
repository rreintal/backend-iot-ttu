using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class SendEmail
{
    // TODO - project id which Repository user wants

    [Required(ErrorMessage = "RecipentEmail is required!")]
    public string RecipentEmail { get; set; } = default!;
    
    [Required]

    public string ProjectId { get; set; } = default!;
}