using Base.Domain;

namespace DAL.DTO.V1;

public class Content : DomainEntityId
{
    public Guid ContentTypeId { get; set; }
    public DAL.DTO.V1.ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public DAL.DTO.V1.News? News { get; set; }
    
    // TODO
    // projectId
    // project

    public Guid LanguageStringId { get; set; }
    public DAL.DTO.V1.LanguageString LanguageString { get; set; } = default!;
}