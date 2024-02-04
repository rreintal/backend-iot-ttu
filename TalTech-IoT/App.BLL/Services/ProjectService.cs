using App.BLL.Contracts;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using Project = BLL.DTO.V1.Project;

namespace App.BLL.Services;

public class ProjectService : BaseEntityService<Project, Domain.Project, IProjectsRepository>, IProjectService
{
    private IAppUOW Uow { get; }
    private IThumbnailService ThumbnailService { get; }
    private IMapper _mapper { get; set; }
    public ProjectService(IAppUOW uow, IMapper<Project, Domain.Project> mapper,  IMapper autoMapper, IThumbnailService thumbnailService) : base(uow.ProjectsRepository, mapper)
    {
        Uow = uow;
        ThumbnailService = thumbnailService;
        _mapper = autoMapper;
    }

    public override Project Add(Project entity)
    {
        var domainEntity = Mapper.Map(entity);

        // CDN stuff
        
        
        var dalResult = Uow.ProjectsRepository.Add(domainEntity);
        return _mapper.Map<Project>(dalResult);
    }
    
    public async Task<Project?> UpdateAsync(UpdateProject entity)
    {
        // TODO: here use the ImageService
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateProject>(entity);
        var updatedDalEntity = await Uow.ProjectsRepository.UpdateAsync(dalEntity);
        var result = _mapper.Map<Project>(updatedDalEntity);
        return result;
    }


    public async Task<IEnumerable<Project>> AllAsync(string? languageCulture)
    {
        return (await Uow.ProjectsRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<Project?> FindAsync(Guid id, string? languageCulture)
    {
        var entity = await Uow.ProjectsRepository.FindAsync(id, languageCulture);
        var result = Mapper.Map(entity);
        return result;
    }

    public async Task<int> FindProjectTotalCount()
    {
        return await Uow.ProjectsRepository.FindProjectTotalCount();
    }
}