using App.Domain.Translations;
using Base.Domain;

namespace App.Domain;

public class Content : DomainEntityId
{
    public Guid ContentTypeId { get; set; }
    public ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public News? News { get; set; }

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public Guid? PageContentId { get; set; }
    public PageContent? PageContent { get; set; }

    public Guid? HomePageBannerId { get; set; }
    public HomePageBanner? HomePageBanner { get; set; }

    public Guid? ContactPersonId { get; set; }
    public ContactPerson? ContactPerson { get; set; }

    public Guid? FeedPageCategoryId { get; set; }
    public FeedPageCategory? FeedPageCategory { get; set; }

    public Guid? FeedPagePostId { get; set; }
    public FeedPagePost? FeedPagePost { get; set; }
    
    public Guid LanguageStringId { get; set; }
    public LanguageString LanguageString { get; set; } = default!;
}