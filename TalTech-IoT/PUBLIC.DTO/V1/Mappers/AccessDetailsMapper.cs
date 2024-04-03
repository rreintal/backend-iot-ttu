using Public.DTO.V1.OpenSourceSolution;
using AccessDetails = BLL.DTO.V1.AccessDetails;

namespace Public.DTO.V1.Mappers;

public class AccessDetailsMapper
{
    public static OpenSourceSolution.AccessDetails Map(AccessDetails entity)
    {
        return new OpenSourceSolution.AccessDetails()
        {
            Email = entity.Email,
            Date = entity.Date,
            OpenSourceSolutionId = entity.OpenSourceSolutionId
        };
    }

    public static AccessDetails Map(RequestOpenSourceSolutionAccess entity)
    {
        return new AccessDetails()
        {
            Email = entity.Email,
            Date = DateTime.UtcNow,
            OpenSourceSolutionId = entity.SolutionId
        };
    }
}