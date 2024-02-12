using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class GetFeedPageTranslated : DomainEntityId
{
    public string FeedPageName { get; set; } = default!;
    public List<GetFeedPageCategoryTranslated> FeedPageCategories { get; set; } = default!;
}