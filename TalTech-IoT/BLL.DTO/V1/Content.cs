using Azure.Core;
using Base.Domain;

namespace BLL.DTO.V1;

public class Content : DomainEntityId
{
    public Guid ContentTypeId { get; set; }
    public BLL.DTO.V1.ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public BLL.DTO.V1.News? News { get; set; }
    
    // TODO
    // projectId
    // project
    public Guid? ProjectId { get; set; }
    public BLL.DTO.V1.Project? Project { get; set; }

    public Guid LanguageStringId { get; set; }
    public BLL.DTO.V1.LanguageString LanguageString { get; set; } = default!;
}