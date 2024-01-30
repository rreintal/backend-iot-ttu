using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

// TODO parem nimi create/update on sama
public class PostNewsDto
{
    //[IncludesAllCultures]
    //[ValidCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    //[IncludesAllCultures]
    //[ValidCultures]
    public List<ContentDto> Body { get; set; } = default!;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Author { get; set; } = default!;
    public string? Image { get; set; }
    
    [MinLength(1, ErrorMessage = RestApiErrorMessages.GeneralMissingTopicArea)]
    public List<TopicArea> TopicAreas { get; set; } = default!;
}
