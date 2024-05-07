using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class OpenSourceSolution : DomainEntityId, IContentEntity
{
    public ICollection<Content> Content { get; set; } = default!;
    public bool Private { get; set; }
    public string Link { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public ICollection<AccessDetails>? AccessDetails { get; set; }
}

//  Title, description, createdAt, private/public, link