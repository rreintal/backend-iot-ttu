using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PageContent
{
    [Required]
    public string PageIdentifier { get; set; } = default!;

    //[ValidCultures]
    //[IncludesAllCultures]
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Title { get; set; } = default!;

    //[ValidCultures]
    //[IncludesAllCultures]
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Body { get; set; } = default!;
    
    
}