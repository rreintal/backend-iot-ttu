using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class News : DomainEntityIdMetaData, IHasTopicAreaEntity, IContentEntity
{
    public ICollection<Content> Content { get; set; } = default!;
    public ICollection<HasTopicArea> HasTopicAreas { get; set; } = default!;

    public string? Image { get; set; }
    public string ThumbnailImage { get; set; } = default!;
    public string Author { get; set; } = default!;
    
    public int ViewCount { get; set; } 
    
    public ICollection<ImageResource>? ImageResources { get; set; }
    
}