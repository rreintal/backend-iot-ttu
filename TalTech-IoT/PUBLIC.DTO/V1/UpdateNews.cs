using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;
public class UpdateNews : DomainEntityId
{
    public string Author { get; set; } = default!;
    
    [ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    
    [ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    public string Image { get; set; } = default!;
    
    [Required]
    [MinLength(1, ErrorMessage = RestApiErrorMessages.GeneralMissingTopicArea)]
    public List<TopicArea> TopicAreas { get; set; } = default!;
    // public DateTime? CreatedAt { get; set; } kas on vaja??
}