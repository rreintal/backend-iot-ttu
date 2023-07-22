namespace Public.DTO.V1;

public class CreateTopicAreaDto
{
    public Guid? ParentTopicId { get; set; }
    public List<ContentDto> Name { get; set; } = default!;
}