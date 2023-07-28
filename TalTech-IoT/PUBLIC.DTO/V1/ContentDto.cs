using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class ContentDto
{
    [Required]
    public string Value { get; set; } = default!;
    
    [Required]
    public string Culture { get; set; } = default!;
}