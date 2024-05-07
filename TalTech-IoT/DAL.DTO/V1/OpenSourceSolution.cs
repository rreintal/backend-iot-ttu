using System.Linq.Expressions;
using Base.Domain;

namespace DAL.DTO.V1;

public class OpenSourceSolution : DomainEntityId
{
    public List<Content> Title { get; set; } = default!;
    public List<Content> Body { get; set; } = default!;
    public bool Private { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Link { get; set; } = default!;
    public ICollection<AccessDetails>? AccessDetails { get; set; }
}