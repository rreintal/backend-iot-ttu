using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using Project = BLL.DTO.V1.Project;

namespace App.BLL.Services;

public class ProjectService : BaseEntityService<Project, Domain.Project, IProjectsRepository>, IProjectService
{
    private IAppUOW Uow { get; }
    public ProjectService(IAppUOW uow, IMapper<Project, Domain.Project> mapper) : base(uow.ProjectsRepository, mapper)
    {
        Uow = uow;
    }

    public Project Add(Project entity)
    {
        var domainEntity = Mapper.Map(entity);
        foreach (var bllTopicArea in entity.TopicAreas)
        {
            var hasTopicAreaId = Guid.NewGuid();
            var hasTopicArea = new App.Domain.HasTopicArea()
            {
                Id = hasTopicAreaId,
                ProjectId = domainEntity!.Id,
                TopicAreaId = bllTopicArea.Id,
            };

            if (domainEntity.HasTopicAreas == null)
            {
                domainEntity.HasTopicAreas = new List<HasTopicArea>()
                {
                    hasTopicArea
                };
            }
            else
            {
                domainEntity.HasTopicAreas.Add(hasTopicArea);
            }
        }

        Uow.ProjectsRepository.Add(domainEntity);

        return entity;
    }
}