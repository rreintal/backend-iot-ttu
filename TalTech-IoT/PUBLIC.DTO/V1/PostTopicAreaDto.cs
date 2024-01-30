using System.ComponentModel.DataAnnotations;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostTopicAreaDto
{
    public Guid? ParentTopicId { get; set; }

    //[ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Name { get; set; } = default!;
    
}