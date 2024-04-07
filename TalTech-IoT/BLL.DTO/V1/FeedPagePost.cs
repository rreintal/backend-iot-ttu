using Base.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;

namespace BLL.DTO.V1;

public class FeedPagePost : DomainEntityId, IContentEntity, IContainsImageResource
{
    public List<Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid FeedPageCategoryId { get; set; }
    public FeedPageCategory? FeedPageCategory { get; set; }

    public List<ImageResource> ImageResources { get; set; } = new List<ImageResource>();
}