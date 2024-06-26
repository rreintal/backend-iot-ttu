using Base.Domain;

namespace DAL.DTO.V1;

public class Content : DomainEntityId
{
    public Guid ContentTypeId { get; set; }
    public ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public News? News { get; set; }
    
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public Guid LanguageStringId { get; set; }
    public LanguageString LanguageString { get; set; } = default!;
 
    public Guid? PageContentId { get; set; }
    public PageContent? PageContent { get; set; }
}