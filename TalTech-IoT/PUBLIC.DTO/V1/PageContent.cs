using System.ComponentModel.DataAnnotations;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PageContent
{
    [Required]
    public string PageIdentifier { get; set; } = default!;

    //[ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;

    //[ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    
    
}