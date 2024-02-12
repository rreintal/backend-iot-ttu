using Base.Domain;
namespace BLL.DTO.V1;
public class FeedPageCategoryWithoutPosts : DomainEntityId
{
    public Guid FeedPageId { get; set; }
    public List<ContentDto> Title { get; set; } = default!;
}
