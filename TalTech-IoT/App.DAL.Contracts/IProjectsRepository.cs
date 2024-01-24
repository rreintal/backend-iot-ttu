using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IProjectsRepository : IBaseTranslateableRepository<App.Domain.Project>, IProjectsRepositoryCustom<App.Domain.Project>
{
    
}

public interface IProjectsRepositoryCustom<TEntity>
{
    // here methods which are shared between repo and service!
    public Task<int> FindProjectTotalCount();
}