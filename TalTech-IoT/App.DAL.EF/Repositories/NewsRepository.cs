using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class NewsRepository : EFBaseRepository<App.Domain.News, AppDbContext>, INewsRepository
{
    private const int DEFAULT_PAGE_SIZE = 5;
    
    public NewsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper) {}
    
    // TODO - TEE DAL OBJECT; et HasTopicArea -> TopicArea?
    // TODO - mapi juba query ajal ära!!!
    // TODO - kas on üldse DAL objecte vaja!?
    public override Domain.News Add(Domain.News entity)
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

    public async Task<News?> FindByIdWithAllTranslationsAsync(Guid Id)
    {
        
        var query = await DbSet.Where(x => x.Id == Id)
            .IncludeHasTopicAreasWithTranslation()
            .IncludeContentWithTranslation()
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }

        return query;
    }

    public async Task<News> Update(UpdateNews entity)
    {
        // Maybe should be await?!
        // TODO - error handling

        // TODO - is it neccesary?! optimize?
        var existingDomainObject = await FindByIdWithAllTranslationsAsync(entity.Id);
        

        // TODO - if there is no existing object with that ID!
        // throw error

        // check if content has changed!
        // TODO - helper function for detecting changes, can use for Project!
        foreach (var lang in LanguageCulture.ALL_LANGUAGES)
        {
            var newBodyValue = entity.GetContentValue(ContentTypes.BODY, lang);
            var newTitleValue = entity.GetContentValue(ContentTypes.TITLE, lang);
    
            var oldBodyValue = existingDomainObject!.GetContentValue(ContentTypes.BODY, lang);
            var oldTitleValue = existingDomainObject.GetContentValue(ContentTypes.TITLE, lang);

            if (oldBodyValue != newBodyValue)
            {
                existingDomainObject.SetContentTranslationValue(ContentTypes.BODY, lang, newBodyValue);
                existingDomainObject.SetBaseLanguage(ContentTypes.BODY, newBodyValue);
            }

            if (oldTitleValue != newTitleValue)
            {
                existingDomainObject.SetContentTranslationValue(ContentTypes.TITLE, lang, newTitleValue);
                existingDomainObject.SetBaseLanguage(ContentTypes.TITLE, newBodyValue);
            }
        }
        // TODO - if it has topicAreas more than 2 levels!?
        // is it relevant? all children are linked with id
        
        // check if topicAreaHasChanged
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
        // TODO - refactor!!!!!
        // remove all previous topicAreas

        // TODO - 
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

        var result = Update(existingDomainObject);
        return result;
    }

    // see peaks vist DAL objekt olema tegelt?!
    // need HasTopicArea-d oleks mpaitud juba TopicArea

    public async Task<IEnumerable<App.Domain.News>> AllAsyncFiltered(int? page, int? size, string languageCulture)
    {
        // TODO - optimize!!!!
        page = page ?? 0;
        size = size ?? DEFAULT_PAGE_SIZE;

        // TODO - error
        return await DbSet
            .AsNoTracking()
            .IncludeHasTopicAreasWithTranslation(languageCulture)
            .IncludeContentWithTranslation(languageCulture)
            .OrderByDescending(x => x.CreatedAt)
                .Skip(page.Value * size.Value)
                .Take(size.Value)
            .ToListAsync();
    }
    
    public async Task<News?> FindAsync(Guid id, string? languageCulture)
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


    public async Task<IEnumerable<News>> AllAsync(string? languageString)
    {
        return await DbSet
            .IncludeHasTopicAreasWithTranslation(languageString)
            .IncludeContentWithTranslation(languageString)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}