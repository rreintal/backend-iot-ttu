using System.ComponentModel.DataAnnotations;

namespace Public.DTO.V1.OpenSourceSolution;

public class RequestOpenSourceSolutionAccess
{
    [MinLength(2)]
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    public Guid SolutionId { get; set; } = default!;
}