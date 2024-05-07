using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class FeedPagePost : DomainEntityId, IContentEntity
{
    public ICollection<Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid FeedPageCategoryId { get; set; }
    public FeedPageCategory? FeedPageCategory { get; set; }
    
    public ICollection<ImageResource>? ImageResources { get; set; }
}