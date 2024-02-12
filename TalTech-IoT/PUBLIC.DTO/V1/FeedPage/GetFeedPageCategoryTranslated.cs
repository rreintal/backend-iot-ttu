using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class GetFeedPageCategoryTranslated : DomainEntityId
{
    public string Title { get; set; } = default!;
    public List<GetFeedPageCategoryPostTranslated> FeedPageCategoryPost { get; set; } = default!;
}