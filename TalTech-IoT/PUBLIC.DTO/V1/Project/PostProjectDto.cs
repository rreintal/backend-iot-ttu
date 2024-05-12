using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostProjectDto
{
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectYear)]
    public int Year { get; set; }

    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectManager)]
    [MinLength(2)]
    [MaxLength(64)]
    public string ProjectManager { get; set; } = default!;
    
    // In Euros
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectVolume)]
    [Range(0, 1000000000)]
    public double ProjectVolume { get; set; }
    
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;

    [Required]
    public bool IsOngoing { get; set; }
}
