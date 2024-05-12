using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostNewsDto
{
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Author { get; set; } = default!;
    public string? Image { get; set; }
    
    [MinLength(1, ErrorMessage = RestApiErrorMessages.GeneralMissingTopicArea)]
    public List<TopicArea> TopicAreas { get; set; } = default!;
}
