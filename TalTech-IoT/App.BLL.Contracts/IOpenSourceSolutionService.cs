using App.DAL.Contracts;
using Base.BLL.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IOpenSourceSolutionService : ITranslateableEntityService<OpenSourceSolution>, IOpenSourceSolutionRepositoryCustom<Domain.OpenSourceSolution>
{
    
}