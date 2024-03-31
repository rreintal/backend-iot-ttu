using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.Content;
using ImageResource = App.Domain.ImageResource;
using News = DAL.DTO.V1.News;

namespace App.DAL.EF.Repositories;

public class NewsRepository : EFBaseRepository<App.Domain.News, AppDbContext>, INewsRepository
{
    private const int DEFAULT_PAGE_SIZE = 5;

    public NewsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper) {}
    
    // TODO - TEE DAL OBJECT; et HasTopicArea -> TopicArea?
    // TODO - mapi juba query ajal ära!!!
    // TODO - kas on üldse DAL objecte vaja!? ei ole, kogu mappimine käib BLL -> Public!
    public override App.Domain.News Add(Domain.News entity)
    {
        
        foreach (var content in entity.Content)
        {
            // Doing this database does not try to add again types to db.
            DbContext.Attach(content.ContentType);
        }

        entity.CreatedAt = DateTime.UtcNow;
        
        var res = DbSet.Add(entity).Entity;
        return res;
    }

    public News Add(News entity)
    {
        var domainEntity = _mapper.Map<App.Domain.News>(entity);

        foreach (var bllTopicArea in entity.TopicAreas)
        {
            var hasTopicAreaId = Guid.NewGuid();
            var hasTopicArea = new App.Domain.HasTopicArea()
            {
                Id = hasTopicAreaId,
                NewsId = domainEntity.Id,
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

        var dalResult = Add(domainEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
    }

    public Domain.News? FindByIdWithAllTranslations(Guid id)
    {
        var query = DbSet.Where(x => x.Id == id)
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .FirstOrDefault();

        if (query == null)
        {
            return null;
        }
        
        return _mapper.Map<Domain.News>(query);
    }

    public override Domain.News Remove(Domain.News entity)
    {
        DbSet.Entry(entity).State = EntityState.Deleted;
        return base.Remove(entity);
    }

    public async Task<News> AddAsync(News entity)
    {
        var domainEntity = _mapper.Map<App.Domain.News>(entity);

        foreach (var bllTopicArea in entity.TopicAreas)
        {
            var hasTopicAreaId = Guid.NewGuid();
            var hasTopicArea = new App.Domain.HasTopicArea()
            {
                Id = hasTopicAreaId,
                NewsId = domainEntity.Id,
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

        var dalResult = Add(domainEntity);
        var result = _mapper.Map<News>(dalResult);
        return result;
    }

    public Task<List<ImageResource>> GetImageResources(Guid id)
    {
        return DbContext.ImageResources.Where(x => x.NewsId == id).ToListAsync();
    }

    public async Task<App.Domain.News?> FindByIdWithAllTranslationsAsync(Guid Id)
    {
        var query = await DbSet.Where(x => x.Id == Id)
            .Include(x => x.ImageResources)
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }
        
        return query;
    }

    public async Task<Domain.News?> FindByIdWithAllTranslationsAsyncNoTracking(Guid Id)
    {
        var query = await DbSet.Where(x => x.Id == Id)
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }
        
        return query;
    }

    public async Task<App.Domain.News?> Update(News dalEntity)
    {
        /*
         var existingDomainObject = FindByIdWithAllTranslations(entity.Id);
        var newDomainObject = _mapper.Map<Domain.News>(existingDomainObject);
        existingDomainObject!.Image = newDomainObject.Image;
        UpdateContentHelper.UpdateContent(existingDomainObject, newDomainObject);
        return base.Update(existingDomainObject);
         */
        var existingDomainObject = await FindByIdWithAllTranslationsAsyncTracking(dalEntity.Id);
        var newDomainObject = _mapper.Map<Domain.News>(dalEntity);

        var entry = DbSet.Entry(existingDomainObject);
        
        // imagine its updated  
        if (existingDomainObject == null)
        {
            return null;
        }
        
        entry.Entity.ImageResources = dalEntity.ImageResources.Select(e => _mapper.Map<ImageResource>(e)).ToList();
        existingDomainObject!.Image = newDomainObject.Image;
        UpdateContentHelper.UpdateContent(existingDomainObject, newDomainObject);
        var result = Update(existingDomainObject);
        return result;
    }

    public async Task<App.Domain.News?> FindByIdWithAllTranslationsAsyncTracking(Guid Id)
    {
        var query = await DbSet.Where(x => x.Id == Id)
            .AsTracking()
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }
        
        return query;
    }
    
    public async Task<IEnumerable<App.Domain.News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageCulture)
    {
        // TODO - optimize!!!!
        filterSet.Size ??= DEFAULT_PAGE_SIZE;
        filterSet.Page ??= 0;
        IQueryable<App.Domain.News> query = DbSet;

        
        if (filterSet.TopicAreaId.HasValue)
        {
            query = query.Where(x => x.HasTopicAreas.Any(hta => hta.TopicAreaId == filterSet.TopicAreaId));
        }

        if (filterSet.IncludeBody.HasValue && !filterSet.IncludeBody.Value)
        {
            
            var result = (await query
                .IncludeHasTopicAreasWithTranslation(languageCulture)
                .IncludeContentWithTitlesTranslation(languageCulture)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(filterSet.Page.Value * filterSet.Size.Value)
                .Take(filterSet.Size.Value)
                .ToListAsync()).Select(e => _mapper.Map<App.Domain.News>(e));
            return result;
        }
        return await query
            .IncludeHasTopicAreasWithTranslation(languageCulture)
            .IncludeContentWithTranslation(languageCulture)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(filterSet.Page.Value * filterSet.Size.Value)
            .Take(filterSet.Size.Value)
            .ToListAsync();
    }
    
    public async Task<App.Domain.News?> FindAsync(Guid id, string? languageCulture)
    {
        var query = await DbSet.Where(x => x.Id == id)
            .IncludeHasTopicAreasWithTranslation(languageCulture)
            .IncludeContentWithTranslation(languageCulture)
            .FirstOrDefaultAsync();
        if (query == null)
        {
            return null;
        }

        return query;
    }


    public async Task<IEnumerable<App.Domain.News>> AllAsync(string? languageString)
    {
        return await DbSet
            .IncludeHasTopicAreasWithTranslation(languageString)
            .IncludeContentWithTranslation(languageString)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> FindNewsTotalCount()
    {
        return await DbSet.CountAsync();
    }

    public async Task<int> FindNewsTotalCount(Guid? TopicAreaId)
    {
        if (TopicAreaId == null)
        {
            return await DbSet.CountAsync();
        }

        return await DbSet
            .Where(x => x.HasTopicAreas.Any(hta => hta.TopicAreaId == TopicAreaId))
            .CountAsync();

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
}