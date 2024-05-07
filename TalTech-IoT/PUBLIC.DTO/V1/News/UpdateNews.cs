using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Base.Domain;

namespace Public.DTO.V1;
public class UpdateNews : DomainEntityId
{
    public string Author { get; set; } = default!;
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Body { get; set; } = default!;
    
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Title { get; set; } = default!;
    
    [Required]
    public string? Image { get; set; }
    
    public List<TopicArea> TopicAreas { get; set; } = default!;
    // public DateTime? CreatedAt { get; set; } kas on vaja??
}