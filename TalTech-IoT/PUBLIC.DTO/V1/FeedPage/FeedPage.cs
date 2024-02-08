using Base.Domain;

namespace Public.DTO.V1.FeedPage;

public class FeedPage : DomainEntityId
{
    public string FeedPageName { get; set; } = default!;
    public List<FeedPageCategory>? FeedPageCategories { get; set; }
}