using Base.Domain;

namespace Public.DTO.V1;
public class UpdateNews : DomainEntityId
{
    public string Author { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;
    public List<ContentDto> Title { get; set; } = default!;
    public string Image { get; set; } = default!;
    public List<TopicArea> TopicAreas { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
}