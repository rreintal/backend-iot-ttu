namespace Public.DTO.V1.OpenSourceSolution;

public class RequestOpenSourceSolutionAccess
{
    public string Email { get; set; } = default!;
    public Guid SolutionId { get; set; } = default!;
}