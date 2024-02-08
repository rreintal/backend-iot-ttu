using Base.Domain;

namespace BLL.DTO.V1;

public class FeedPagePost : DomainEntityId
{
    public List<Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid FeedPageCategoryId { get; set; }
    public FeedPageCategory? FeedPageCategory { get; set; }
}