using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class FeedPageCategory : DomainEntityId, IContentEntity
{
    public Guid FeedPageId { get; set; }
    public FeedPage? FeedPage { get; set; }
    
    public List<Content> Content { get; set; } = default!;
    public List<FeedPagePost>? FeedPagePosts { get; set; }
}