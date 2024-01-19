using Base.Domain;

namespace App.Domain;

public class Project : DomainEntityIdMetaData
{
    public int Year { get; set; } 
    public double PriceVolume { get; set; } 
    public string ProjectManager { get; set; } = default!;
    
    public string? Image { get; set; }
    public string? ThumbnailImage { get; set; }

    public ICollection<HasTopicArea> HasTopicAreas { get; set; } = default!;
    // title, content
    public ICollection<Content> Content { get; set; } = default!;
    
    
}