using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class FeedPagePost : DomainEntityId
{
    public Guid FeedPageCategoryId { get; set; }
    public List<ContentDto> Content { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
}