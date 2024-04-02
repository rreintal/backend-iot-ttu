using BLL.DTO.V1;

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
}