using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class Project : DomainEntityIdMetaData, IContentEntity, IContainsImageResource
{
    public int Year { get; set; } 
    public double ProjectVolume { get; set; } 
    public string ProjectManager { get; set; } = default!;

    // TODO: add CreatedAt
    public bool IsOngoing { get; set; }
    
    public ICollection<ImageResource>? ImageResources { get; set; }
    public ICollection<Content> Content { get; set; } = default!;

    public int ViewCount { get; set; }
    
}