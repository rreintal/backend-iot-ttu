using Base.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;

namespace BLL.DTO.V1;

public class UpdateProject : DomainEntityId, IContentEntity, IContainsImageResource
{
    public int? Year { get; set; } = default!;
    public string? ProjectManager { get; set; }
    public double? ProjectVolume { get; set; }

    public List<Content> Content { get; set; } = default!;
    
    public List<ImageResource> ImageResources { get; set; } = new List<ImageResource>();
    
}