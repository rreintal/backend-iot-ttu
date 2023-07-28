using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class PostTopicAreaDto
{
    public Guid? ParentTopicId { get; set; }

    [Required(ErrorMessage = "TopicArea must have translations in both languages.")]
    public List<ContentDto> Name { get; set; } = default!;
    
}