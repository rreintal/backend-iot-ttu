using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class PageContent : DomainEntityId, IContentEntity
{
    public string PageIdentifier { get; set; } = default!;
    public List<Content> Content { get; set; } = default!;
}