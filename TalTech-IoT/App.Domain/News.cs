using Base.Domain;

namespace App.Domain;

public class News : DomainEntityIdMetaData
{
    public ICollection<Content> Content { get; set; } = default!;
    public ICollection<HasTopicArea> HasTopicAreas { get; set; } = default!;

    public string Image { get; set; } = default!;
    public string ThumbnailImage { get; set; } = default!;
    public string Author { get; set; } = default!;
}