using Base.Domain;

namespace App.Domain;

public class ImageResource : DomainEntityId
{
    public string Link { get; set; } = default!; // https://www.myImageRepository.ee/images/0001-0000-0000-0000.jpg
    public Guid? NewsId { get; set; }
    public News? News { get; set; } = default!;
    
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid? PageContentId { get; set; }
    public PageContent? PageContent { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}