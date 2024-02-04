using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
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

    public Project Add(Project entity)
    {
        var domainEntity = Mapper.Map(entity);
        
        // Add Thumbnail
        // TODO: check if domainEntity is null. probably not but still check
        // TODO: think through about the mappers structure
        
        if (domainEntity!.Image != null)
        {
            domainEntity!.ThumbnailImage = ThumbnailService.Compress(domainEntity.Image);   
        }

        Uow.ProjectsRepository.Add(domainEntity);

        return entity;
    }


    public async Task<IEnumerable<Project>> AllAsync(string? languageCulture)
    {
        return (await Uow.ProjectsRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<Project?> FindAsync(Guid id, string? languageCulture)
    {
        //Uow.ProjectsRepository.FindAsync(languageCulture, id);
        var entity = await Uow.ProjectsRepository.FindAsync(id, languageCulture);
        var result = Mapper.Map(entity);
        return result;
    }

    public async Task<int> FindProjectTotalCount()
    {
        return await Uow.ProjectsRepository.FindProjectTotalCount();
    }

    public async Task<Project?> UpdateAsync(UpdateProject entity)
    {
        // TODO: here use the ImageService
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateProject>(entity);
        var updatedDalEntity = await Uow.ProjectsRepository.UpdateAsync(dalEntity);
        var result = _mapper.Map<Project>(updatedDalEntity);
        return result;
    }
}