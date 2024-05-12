using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostTopicAreaDto
{
    
    [IncludesAllCultures]
    public List<ContentDto> Name { get; set; } = default!;
    
}