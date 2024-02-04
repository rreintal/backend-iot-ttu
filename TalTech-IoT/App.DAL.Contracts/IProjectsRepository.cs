using Base.DAL.EF.Contracts;
using DAL.DTO.V1;
using Project = App.Domain.Project;

namespace App.DAL.Contracts;

public interface IProjectsRepository : IBaseTranslateableRepository<App.Domain.Project>, IProjectsRepositoryCustom<App.Domain.Project>
{
    public Task<Project?> UpdateAsync(UpdateProject entity);

    public Task<Project?> FindByIdAsyncWithAllTranslations(Guid id);
}

public interface IProjectsRepositoryCustom<TEntity>
{
    // here methods which are shared between repo and service!
    public Task<int> FindProjectTotalCount();
}