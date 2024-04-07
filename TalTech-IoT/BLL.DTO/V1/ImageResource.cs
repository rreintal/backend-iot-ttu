using Base.Domain;

namespace BLL.DTO.V1;

public class ImageResource : DomainEntityId
{
    public string Link { get; set; } = default!; // https://www.myImageRepository.ee/images/0001-0000-0000-0000.jpg
    public Guid? NewsId { get; set; }
    public News? News { get; set; } = default!;

    public Guid? HomePageBannerId { get; set; }
    public HomePageBanner? HomePageBanner { get; set; }
    
    public Guid? PartnerImageId { get; set; }
    public PartnerImage? PartnerImage { get; set; }
    
    public Guid? FeedPagePostId { get; set; }
    public FeedPagePost? FeedPagePost { get; set; }
    
    public Guid? PageContentId { get; set; }
    public PageContent? PageContent { get; set; }
}