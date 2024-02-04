using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Base.Domain;

namespace Public.DTO.V1;

public class UpdateProject : DomainEntityId
{
    //[Range(1000, 3000)] 
    public int? Year { get; set; }

    [MinLength(2)]
    [MaxLength(64)]
    public string? ProjectManager { get; set; }
    
    // In Euros
    [Range(0, 1000000000)]
    public double? ProjectVolume { get; set; }
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Title { get; set; } = default!;
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Body { get; set; } = default!;
}