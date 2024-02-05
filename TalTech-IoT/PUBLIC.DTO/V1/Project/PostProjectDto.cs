using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostProjectDto
{
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectYear)]
    //[Range(1000, 3000)] // ??
    public int Year { get; set; } = default!;

    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectManager)]
    [MinLength(2)]
    [MaxLength(64)]
    public string ProjectManager { get; set; } = default!;
    
    // In Euros
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectVolume)]
    [Range(0, 1000000000)]
    public double ProjectVolume { get; set; }
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Title { get; set; } = default!;
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Body { get; set; } = default!;

    [Required]
    public bool IsOngoing { get; set; } = default!;


    // TODO - seda pole ju siin vaja, sest date pannakse alles siis kui db salvestan
    //public DateTime CreatedAt { get; set; } = default!;
}
