using Base.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;

namespace BLL.DTO.V1;

public class PageContent : DomainEntityId, IContentEntity, IContainsImageResource
{
    public string PageIdentifier { get; set; } = default!;
    public List<Content> Content { get; set; } = default!;
    public List<ImageResource> ImageResources { get; set; } = new List<ImageResource>();
}