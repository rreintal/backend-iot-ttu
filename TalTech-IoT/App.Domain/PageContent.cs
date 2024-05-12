using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class PageContent : DomainEntityId, IContentEntity, IContainsImageResource
{
    public string PageIdentifier { get; set; } = default!;
    
    public ICollection<Content> Content { get; set; } = default!; // contains Body + Titile
    public ICollection<ImageResource>? ImageResources { get; set; }
    
}