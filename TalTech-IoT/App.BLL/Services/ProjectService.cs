using System.Drawing;
using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
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
    
    private IImageStorageService _imageStorageService { get; }
    private IMapper _mapper { get;}
    public ProjectService(IAppUOW uow, IMapper<Project, Domain.Project> mapper,  IMapper autoMapper, IThumbnailService thumbnailService) : base(uow.ProjectsRepository, mapper)
    {
        Uow = uow;
        ThumbnailService = thumbnailService;
        _mapper = autoMapper;
        _imageStorageService = new ImageStorageService.ImageStorageService();
    }

    public override Project Add(Project entity)
    {
        // CDN stuff
        var result = _imageStorageService.ProccessSave(entity);
        if (result != null && result.SavedLinks != null)
        {
            entity.ImageResources = result.SavedLinks.Select(e => new ImageResource()
            {
                ProjectId = entity.Id,
                Link = e
            }).ToList();
        }
        
        var domainEntity = Mapper.Map(entity);
        var dalResult = Uow.ProjectsRepository.Add(domainEntity);
        return _mapper.Map<Project>(dalResult);
    }
    
    public async Task<Project?> UpdateAsync(UpdateProject entity)
    {
        var existingEntity = await Uow.ProjectsRepository.FindByIdAsyncWithAllTranslations(entity.Id);
        if (existingEntity == null)
        {
            return null;
        }

        if (existingEntity.ImageResources != null)
        {
            entity.ImageResources = existingEntity.ImageResources.Select(e => new ImageResource()
            {
                ProjectId = entity.Id,
                Link = e.Link
            }).ToList();
        }
        
        var updateResult = _imageStorageService.ProccessUpdate(entity);
        if (updateResult != null)
        {
            if (updateResult.DeletedLinks != null)
            {
                var deleteContent = new DeleteContent()
                {
                    Links = updateResult.DeletedLinks
                };
                _imageStorageService.ProcessDelete(deleteContent);
            }

            if (updateResult.SavedLinks != null)
            {
                entity.ImageResources = updateResult.SavedLinks.Select(e => new ImageResource()
                {
                    ProjectId = entity.Id,
                    Link = e
                }).ToList();
            }
        }
        
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateProject>(entity);
        var updatedDalEntity = await Uow.ProjectsRepository.UpdateAsync(dalEntity);
        var result = _mapper.Map<Project>(updatedDalEntity);
        return result;
    }

    public override async Task<Project?> RemoveAsync(Guid id)
    {
        var existingEntity = await Uow.ProjectsRepository.FindByIdWithImageResources(id);
        if (existingEntity == null)
        {
            return null;
        }

        if (existingEntity.ImageResources != null)
        {
            var deleteContent = new DeleteContent()
            {
                Links = existingEntity.ImageResources.Select(e => e.Link).ToList()
            };
            _imageStorageService.ProcessDelete(deleteContent);
        }
        
        return await base.RemoveAsync(id);
    }

    public async Task<Project?> FindByIdAsyncAllLanguages(Guid id)
    {
        var dalObject = await Uow.ProjectsRepository.FindByIdAsyncWithAllTranslations(id);
        return _mapper.Map<Project>(dalObject);
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

    public async Task<bool> ChangeProjectStatus(Guid id, bool isOngoing)
    {
        return await Uow.ProjectsRepository.ChangeProjectStatus(id, isOngoing);
    }

    public async Task IncrementViewCount(Guid id)
    {
        await Uow.ProjectsRepository.IncrementViewCount(id);
    }
}