using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1.FeedPage;

public class FeedPagePost : DomainEntityId
{
    public Guid FeedPageCategoryId { get; set; }
    
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}