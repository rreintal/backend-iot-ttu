using App.DAL.Contracts;
using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IProjectService : ITranslateableEntityService<Project>, IProjectsRepositoryCustom<Project>
{
    public Task<Project?> UpdateAsync(UpdateProject entity);

    public Task<Project?> FindByIdAsyncAllLanguages(Guid id);
}