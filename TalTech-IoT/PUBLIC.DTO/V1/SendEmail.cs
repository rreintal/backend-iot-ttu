using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;

namespace Public.DTO.V1;

public class SendEmail
{
    // TODO - project id which Repository user wants

    [Required(ErrorMessage = RestApiErrorMessages.MissingMailRecipent)]
    public string RecipentEmail { get; set; } = default!;
    
    [Required]

    public string ProjectId { get; set; } = default!;
}