using Base.Domain;

namespace DAL.DTO.V1;

public class Project : DomainEntityId
{
    public int Year { get; set; } = default!;
    public string ProjectManager { get; set; } = default!;
    public double ProjectVolume { get; set; } = default!;
    public bool IsOngoing { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Content> Content { get; set; } = default!;
}