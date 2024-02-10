using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class FeedPagePost : DomainEntityId, IContentEntity
{
    public List<Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid FeedPageCategoryId { get; set; }
    public FeedPageCategory? FeedPageCategory { get; set; }
}