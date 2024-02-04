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
                existingDomainObject.SetBaseLanguage(ContentTypes.BODY, newBodyValue);
            }
            
            if (isTitleContentChanged)
            {
                existingDomainObject.SetContentTranslationValue(ContentTypes.TITLE, lang, newTitleValue);
                existingDomainObject.SetBaseLanguage(ContentTypes.TITLE, newBodyValue);
            }
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

    public Task<Project?> FindByIdAsyncWithAllTranslations(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Project?> FindAsyncByIdWithAllTranslations(Guid id)
    {
        return await DbSet.Where(e => e.Id == id)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }
}