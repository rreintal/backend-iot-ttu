using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class FeedPageCategory : DomainEntityId, IContentEntity
{
    // Only title!
    public Guid FeedPageId { get; set; }
    public FeedPage? FeedPage { get; set; }
    
    public ICollection<Content> Content { get; set; } = default!;
    public ICollection<FeedPagePost>? FeedPagePosts { get; set; }
    
    
}