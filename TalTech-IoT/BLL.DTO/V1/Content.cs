using Azure.Core;
using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class Content : DomainEntityId, IContent
{
    public Guid ContentTypeId { get; set; }
    public BLL.DTO.V1.ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public BLL.DTO.V1.News? News { get; set; }
    
    public Guid? ProjectId { get; set; }
    public BLL.DTO.V1.Project? Project { get; set; }

    public Guid LanguageStringId { get; set; }
    public BLL.DTO.V1.LanguageString LanguageString { get; set; } = default!;
 
    public Guid? PageContentId { get; set; }
    public BLL.DTO.V1.PageContent? PageContent { get; set; }
}