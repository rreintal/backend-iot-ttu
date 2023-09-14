using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

// TODO parem nimi create/update on sama
public class PostNewsDto
{
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;

    [Required]
    public string Author { get; set; } = default!;
    
    // TODO - if image is not sent, then set default image!
    public string Image { get; set; } = default!;
    
    //[Required(ErrorMessage = "News must have at least one topic area.")]
    [AtleastOneTopicArea(ErrorMessage = "News must have at least one topic area.")]
    public List<TopicArea> TopicAreas { get; set; } = default!;
    
    
    
    public class AtleastOneTopicArea : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is List<TopicArea> topicArea && topicArea.Count > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("News must have at least one topic area.");
        }
    }
}
