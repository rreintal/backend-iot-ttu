using Base.Domain;

namespace BLL.DTO.V1;

public class FeedPage : DomainEntityId
{
    public string FeedPageName { get; set; } = default!;
    public List<FeedPageCategory>? FeedPageCategories { get; set; }
}