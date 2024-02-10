using App.Domain;
using Base.Domain;

namespace DAL.DTO.V1;

public class UpdateNews : DomainEntityId
{
    public string Author { get; set; } = default!;
    
    public List<DAL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;
    public List<Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; } = default!;
    
}