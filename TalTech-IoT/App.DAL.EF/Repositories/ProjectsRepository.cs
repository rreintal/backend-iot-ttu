using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;
using Project = App.Domain.Project;

namespace App.DAL.EF.Repositories;

public class ProjectsRepository : EFBaseRepository<App.Domain.Project, AppDbContext>, IProjectsRepository
{
    
    public ProjectsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }
    
    public async Task<Project?> UpdateAsync(UpdateProject entity)
    {
        var existingDomainObject = await FindAsyncByIdWithAllTranslations(entity.Id);

        if (existingDomainObject == null)
        {
            return null;
        }
        var cults = LanguageCulture.ALL_LANGUAGES;
        
        foreach (var lang in cults)
        {
            var newBodyValue = entity.GetContentValue(ContentTypes.BODY, lang);
            var newTitleValue = entity.GetContentValue(ContentTypes.TITLE, lang);
    
            var oldBodyValue = existingDomainObject!.GetContentValue(ContentTypes.BODY, lang);
            var oldTitleValue = existingDomainObject.GetContentValue(ContentTypes.TITLE, lang);

            var isBodyValueChanged = oldBodyValue != newBodyValue;
            var isTitleContentChanged = oldTitleValue != newTitleValue;
            
            if (isBodyValueChanged)
            {
                existingDomainObject.SetContentTranslationValue(ContentTypes.BODY, lang, newBodyValue);
            }
            
            if (isTitleContentChanged)
            {
                existingDomainObject.SetContentTranslationValue(ContentTypes.TITLE, lang, newTitleValue);
            }
        }
        // TODO: map to domain object and then add or smth!

        existingDomainObject.ProjectManager = entity.ProjectManager;
        if (entity.ProjectVolume != null)
        {
            existingDomainObject.ProjectVolume = entity.ProjectVolume.Value;    
        }

        if (entity.Year != null)
        {
            existingDomainObject.Year = entity.Year.Value;
        }

        var updateResult = Update(existingDomainObject);
        var result = _mapper.Map<Project>(updateResult);
        return result;
    }

    public override Project Add(Project entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.ContentTypes.Attach(content.ContentType);
        }

        entity.CreatedAt = DateTime.UtcNow;
        return base.Add(entity);
    }

    public async Task<IEnumerable<Project>> AllAsync(string? languageString)
    {
        var res = await DbSet
            .Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageString))
            .ToListAsync();
        return res;
    }

    public async Task<Project?> FindAsync(Guid id, string? languageCulture)
    {
        var query = await DbSet.Where(x => x.Id == id)
            .Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .FirstOrDefaultAsync();
        if (query == null)
        {
            return null;
        }

        return query;
    }

    public async Task<int> FindProjectTotalCount()
    {
         return await DbSet.CountAsync();
    }

    public async Task<bool> ChangeProjectStatus(Guid id, bool isOngoing)
    {
        // TODO: better approach but how to check if save is successful?
         // TODO: concurrency issues, if something is saved at the same time, then it will be > 0??
        var entity = new Project() { Id = id, IsOngoing = isOngoing};
        DbSet.Attach(entity);
        DbSet.Entry(entity).Property(x => x.IsOngoing).IsModified = true;
        var result = await DbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task IncrementViewCount(Guid id)
    {
        var entity = await FindAsync(id);
        if (entity != null)
        {
            entity.ViewCount++;
            DbSet.Update(entity);
        }
    }

    public async Task<global::DAL.DTO.V1.Project?> FindByIdAsyncWithAllTranslations(Guid id)
    {
        var domainEntity = await DbSet.Where(e => e.Id == id)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
        return _mapper.Map<global::DAL.DTO.V1.Project>(domainEntity);
    }

    public async Task<Project?> FindAsyncByIdWithAllTranslations(Guid id)
    {
        return await DbSet.Where(e => e.Id == id)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }
}