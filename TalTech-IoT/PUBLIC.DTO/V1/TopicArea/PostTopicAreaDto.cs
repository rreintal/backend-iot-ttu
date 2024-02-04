using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostTopicAreaDto
{
    public Guid? ParentTopicId { get; set; }

    //[ValidCultures]
    //[IncludesAllCultures]
    [MinLength(2, ErrorMessage = RestApiErrorMessages.GeneralMissingTranslationValue)]
    public List<ContentDto> Name { get; set; } = default!;
    
}