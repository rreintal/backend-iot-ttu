using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1.FeedPage;

public class FeedPageCategory : DomainEntityId
{
    public Guid FeedPageId { get; set; }
    
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    public List<FeedPagePost>? FeedPageCategoryPosts { get; set; }
}