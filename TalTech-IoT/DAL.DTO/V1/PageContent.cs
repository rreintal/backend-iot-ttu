using Base.Domain;

namespace DAL.DTO.V1;

public class PageContent : DomainEntityId
{
    public string PageIdentifier { get; set; } = default!;
    public List<Content> Content { get; set; } = default!;
}