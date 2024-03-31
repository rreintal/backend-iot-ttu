using Base.DAL.EF.Contracts;
using DAL.DTO.V1;
using Project = App.Domain.Project;

namespace App.DAL.Contracts;

public interface IProjectsRepository : IBaseTranslateableRepository<App.Domain.Project>, IProjectsRepositoryCustom<App.Domain.Project>
{
    public Task<Project?> UpdateAsync(UpdateProject entity);

    public Task<global::DAL.DTO.V1.Project?> FindByIdAsyncWithAllTranslations(Guid id);
}

public interface IProjectsRepositoryCustom<TEntity>
{
    public Task<int> FindProjectTotalCount();
    public Task<bool> ChangeProjectStatus(Guid id, bool isOngoing);

    public Task IncrementViewCount(Guid id);
}