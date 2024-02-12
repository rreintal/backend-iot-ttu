using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class GetFeedPageCategoryPostTranslated : DomainEntityId
{
    public Guid FeedPageCategoryId { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
}