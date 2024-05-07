namespace Public.DTO.V1.OpenSourceSolution;

public class AccessDetails
{
    public Guid OpenSourceSolutionId { get; set; }
    public string Email { get; set; } = default!;
    public DateTime Date { get; set; }
}