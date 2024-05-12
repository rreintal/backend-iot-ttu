using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;
public class UpdateNews : DomainEntityId
{
    [MinLength(2)]
    [MaxLength(64)]
    public string Author { get; set; } = default!;
    
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    [Required]
    public string? Image { get; set; }
    
    public List<TopicArea> TopicAreas { get; set; } = default!;
}