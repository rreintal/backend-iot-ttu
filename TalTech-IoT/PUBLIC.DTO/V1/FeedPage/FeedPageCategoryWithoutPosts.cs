using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class FeedPageCategoryWithoutPosts : DomainEntityId
{
    public Guid FeedPageId { get; set; }
    public List<ContentDto> Title { get; set; } = default!;
}