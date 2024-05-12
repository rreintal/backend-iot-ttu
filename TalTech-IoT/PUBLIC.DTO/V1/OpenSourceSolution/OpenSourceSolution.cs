using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1.OpenSourceSolution;

// TODO: add length constraints
public class OpenSourceSolution : DomainEntityId
{
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    
    [Required]
    [MinLength(10)]
    [MaxLength(3000)]
    public string Link { get; set; } = default!;
    
    [Required]
    public bool Private { get; set; }
    public DateTime CreatedAt { get; set; }
}