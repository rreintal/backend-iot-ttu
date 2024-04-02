using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class OpenSourceSolution : DomainEntityId, IContentEntity
{
    public List<Content> Content { get; set; } = default!;
    public bool Private { get; set; }
    public string Link { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public ICollection<AccessDetails>? AccessDetails { get; set; }
}