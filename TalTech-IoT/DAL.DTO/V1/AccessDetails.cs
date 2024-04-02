using Base.Domain;

namespace DAL.DTO.V1;

public class AccessDetails : DomainEntityId
{
    public Guid OpenSourceSolutionId { get; set; }
    public OpenSourceSolution? OpenSourceSolution { get; set; }
    public DateTime Date { get; set; }
    public string Email { get; set; } = default!;
}