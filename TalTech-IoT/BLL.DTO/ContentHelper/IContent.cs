namespace BLL.DTO.ContentHelper;

public interface IContent
{
    public Guid ContentTypeId { get; set; }
    public BLL.DTO.V1.ContentType? ContentType { get; set; }

    public Guid? NewsId { get; set; }
    public BLL.DTO.V1.News? News { get; set; }
    
    public Guid? ProjectId { get; set; }
    public BLL.DTO.V1.Project? Project { get; set; }

    public Guid LanguageStringId { get; set; }
    public BLL.DTO.V1.LanguageString LanguageString { get; set; }
    
    public Guid? PageContentId { get; set; }
    public BLL.DTO.V1.PageContent? PageContent { get; set; }
}