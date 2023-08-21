using Base.Domain;

namespace App.Domain;

public class Project : DomainEntityIdMetaData
{
    public int Year { get; set; } = default!;
    public double PriceVolume { get; set; } = default!;
    public string ProjectManager { get; set; } = default!;
    
    public string Image { get; set; } = default!;
    public string ThumbnailImage { get; set; } = default!;

    public ICollection<HasTopicArea> HasTopicAreas { get; set; } = default!;
    // title, content
    public ICollection<Content> Content { get; set; } = default!;
    
    
}