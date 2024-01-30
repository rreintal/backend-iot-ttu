using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1;

public class GetProject
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    
    public string? Image { get; set; }
    public DateTime? CreatedAt { get; set; }

    public double ProjectVolume { get; set; }

    public string ProjectManager { get; set; } = default!;
    
    public int Year { get; set; }
    
    public List<Public.DTO.V1.GetTopicArea> TopicAreas { get; set; } = default!;

}