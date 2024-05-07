using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IOpenSourceSolutionRepository : IBaseTranslateableRepository<OpenSourceSolution>, IOpenSourceSolutionRepositoryCustom<OpenSourceSolution>
{
    public Task<List<OpenSourceSolution>> AllAsyncWithStatistics();
}

public interface IOpenSourceSolutionRepositoryCustom<TEntity>
{
    public Task<int> GetCount();
}