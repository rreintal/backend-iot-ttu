using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class ContentDto
{
    public ContentDto()
    {
    }

    public ContentDto(string culture, string value)
    {
        Culture = culture;
        Value = value;
    }

    [Required]
    [MinLength(1)]
    [MaxLength(10000000)] // TODO: mis on max length? kas base64 loeb?  
    public string Value { get; set; } = default!;
    
    [Required]
    [MinLength(2)]
    [MaxLength(2)]
    [ValidCultures]
    public string Culture { get; set; } = default!;
}