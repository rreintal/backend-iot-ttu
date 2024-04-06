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
            var hasTopicArea = new HasTopicArea()
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
            var hasTopicArea = new HasTopicArea()
            {
                Id = hasTopicAreaId,
                NewsId = domainEntity.Id,
                TopicAreaId = bllTopicArea.Id,
            };

            domainEntity.HasTopicAreas.Add(hasTopicArea);
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
    
    
    public async Task<App.Domain.News?> Update(News dalEntity)
    {
        var existingDomainObject = await FindByIdWithAllTranslationsAsyncTracking(dalEntity.Id);
        if (existingDomainObject == null)
        {
            return null;
        }
        var newDomainObject = _mapper.Map<Domain.News>(dalEntity);
        UpdateContentHelper.UpdateContent(existingDomainObject, newDomainObject);
        
        // Update properties
        existingDomainObject.Author = newDomainObject.Author;

        // If image is not null. then thumbnail is also updated in service level
        if (dalEntity.Image != null)
        {
            existingDomainObject.Image = newDomainObject.Image;
            existingDomainObject.ThumbnailImage = newDomainObject.ThumbnailImage;
        }
        
        

        var newTopicAreaIds = dalEntity.TopicAreas.Select(ta => ta.Id).ToList();

        var toRemove = existingDomainObject.HasTopicAreas
            .Where(hta => !newTopicAreaIds.Contains(hta.TopicAreaId))
            .ToList();
        
        foreach (var removeItem in toRemove)
        {
            DbContext.HasTopicAreas.Remove(removeItem);
        }

        var currentTopicAreaIds = existingDomainObject.HasTopicAreas.Select(hta => hta.TopicAreaId).ToList();
        var toAdd = newTopicAreaIds.Except(currentTopicAreaIds).ToList();
        foreach (var addId in toAdd)
        {
            
            var newItem = new HasTopicArea()
            {
                TopicAreaId = addId,
                NewsId = existingDomainObject.Id
            };
            DbContext.HasTopicAreas.Entry(newItem).State = EntityState.Added;
            existingDomainObject.HasTopicAreas.Add(newItem);
        }

        return existingDomainObject;
    }
    
    
    /*
    public async Task<App.Domain.News?> Update(News dalEntity)
    {
        var existingDomainObject = await FindByIdWithAllTranslationsAsyncTracking(dalEntity.Id);
        if (existingDomainObject == null)
        {
            return null;
        }

        var newEntity = _mapper.Map<Domain.News>(dalEntity);
        UpdateContentHelper.UpdateContent(existingDomainObject, newEntity);

        // Assume _mapper.Map has been configured to appropriately handle all necessary properties, including collections.
        //_mapper.Map(dalEntity, existingDomainObject);
        
        return existingDomainObject;
    }
    */
    
    public async Task<App.Domain.News?> FindByIdWithAllTranslationsAsyncTracking(Guid Id)
    {
        // This is used leverage the EF tracking functionality to update LanguageString just by modifying the properties.
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