using App.Domain;
using Base.Domain;

namespace DAL.DTO.V1;

public class UpdateProject : DomainEntityId
{
    public int? Year { get; set; }
    public string? ProjectManager { get; set; }
    public double? ProjectVolume { get; set; }
    
    public List<DAL.DTO.V1.Content> Content { get; set; } = default!;
    public List<ImageResource>? ImageResources { get; set; }
    
}