namespace Public.DTO.V1;

public class PostTopicAreaDto
{
    public Guid? ParentTopicId { get; set; }
    public List<ContentDto> Name { get; set; } = default!;
    
}