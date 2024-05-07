using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class HomePageBanner : DomainEntityId, IContentEntity
{
    public ICollection<Content> Content { get; set; } = default!;
    public string Image { get; set; } = default!;

    public int SequenceNumber { get; set; } = default!;
    
    public ImageResource? ImageResources { get; set; }
    
}