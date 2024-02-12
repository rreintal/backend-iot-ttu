using App.Domain;
using Base.Domain;
using BLL.DTO.ContentHelper;
using Contracts;

namespace BLL.DTO.V1;

public class UpdateNews : DomainEntityId, IContentEntity
{
    public string Author { get; set; } = default!;
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;
    public List<BLL.DTO.V1.Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; } = default!;
}