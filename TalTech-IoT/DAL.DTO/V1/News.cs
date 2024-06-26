using Base.Domain;

namespace DAL.DTO.V1;

public class News : DomainEntityId
{
    public List<DAL.DTO.V1.Content> Content { get; set; } = default!;
    public List<DAL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;
    
    public List<DAL.DTO.V1.ImageResource> ImageResources { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public string Author { get; set; } = default!;
    public string? Image { get; set; }

    public string? ThumbnailImage { get; set; }
}