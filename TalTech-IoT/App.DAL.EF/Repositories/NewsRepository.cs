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

    public async Task<App.Domain.News?> FindByIdWithAllTranslationsAsync(Guid Id)
    {
        var query = await DbSet.Where(x => x.Id == Id)
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .AsTracking()
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }
        
        return query;
    }

    public async Task<App.Domain.News?> Update(UpdateNews dalEntity)
    {
        var existingDomainObject = await FindByIdWithAllTranslationsAsync(dalEntity.Id);
        var newDomainObject = _mapper.Map<Domain.News>(dalEntity);
        
        // imagine its updated  
        if (existingDomainObject == null)
        {
            return null;
        }
        
        existingDomainObject!.Image = newDomainObject.Image;
        UpdateContentHelper.UpdateContent(existingDomainObject, newDomainObject);
        var result = Update(existingDomainObject);
        return result;


        // TODO - if it has topicAreas more than 2 levels!?
        // is it relevant? all children are linked with id
        
        // check if topicAreaHasChanged
        /*
        var updateTopicAreas = true;
        foreach (var updateDtoTopicArea in entity.TopicAreas)
        {
            if (existingDomainObject!.HasTopicAreas.FirstOrDefault(ta => ta.TopicAreaId == updateDtoTopicArea.Id) ==
                null)
            {
                updateTopicAreas = true;
                break;
            }
        }
        */
        // TODO - refactor!!!!!
        // remove all previous topicAreas

        // TODO - 
        /*
        foreach (var ta in existingDomainObject!.HasTopicAreas)
        {
            DbContext.HasTopicAreas.Attach(ta);
            DbContext.Entry(ta).State = EntityState.Deleted;
        }
        
        // update topicAreas
        if (updateTopicAreas)
        {
            var newTopicAreas = new List<HasTopicArea>();
            foreach (var bllTa in entity.TopicAreas)
            {
                var hasTopicAreaId = Guid.NewGuid();
                var hasTopicArea = new App.Domain.HasTopicArea()
                {
                    Id = hasTopicAreaId,
                    NewsId = existingDomainObject!.Id,
                    TopicAreaId = bllTa.Id
                };
                newTopicAreas.Add(hasTopicArea);
            }

            // TODO - maybe check if the HasTopicAreas is not null?
            // is it relevant, as its mandatory to have atelast one TopicArea
            existingDomainObject!.HasTopicAreas = newTopicAreas;
        }
        
        // TODO - what is going on here?!
        DbContext.HasTopicAreas.AddRange(existingDomainObject.HasTopicAreas);
        existingDomainObject.Author = "a";
        */
        //var result = Update(existingDomainObject);
        //return result;
    }
    

    // see peaks vist DAL objekt olema tegelt?!
    // need HasTopicArea-d oleks mpaitud juba TopicArea

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

        if (filterSet.IncludeBody.HasValue)
        {
            
            var result = (await query
                .IncludeHasTopicAreasWithTranslation()
                .IncludeContentWithTitlesTranslation()
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
}