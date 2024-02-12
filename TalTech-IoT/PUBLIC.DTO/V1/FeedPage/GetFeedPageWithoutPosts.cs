using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class GetFeedPageCategoryWithoutPosts : DomainEntityId
{
    public string Title { get; set; } = default!;
}