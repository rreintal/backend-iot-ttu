using Base.Domain;

namespace Public.DTO.V1.OpenSourceSolution;

public class OpenSourceSolutionRequestInfo : DomainEntityId
{
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Link { get; set; } = default!;
    public bool Private { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AccessDetails>? AccessDetails { get; set; } 
}