using Base.Domain;

namespace BLL.DTO.V1;

public class Project : DomainEntityId
{
    public int Year { get; set; } = default!;
    public string ProjectManager { get; set; } = default!;
    public double ProjectVolume { get; set; } = default!;
    public string Image { get; set; } = default!;
    
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

    // body + title
    public List<Content> Content { get; set; } = default!;
    
}