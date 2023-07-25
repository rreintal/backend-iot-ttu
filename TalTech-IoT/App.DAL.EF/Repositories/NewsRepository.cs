using App.DAL.Contracts;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class NewsRepository : EFBaseRepository<App.Domain.News, AppDbContext>, INewsRepository
{
    public NewsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }

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

    public async override Task<Domain.News?> FindAsync(Guid id)
    {
        var query = await DbSet.Where(x => x.Id == id)
            .Include(x => x.HasTopicAreas)
            .ThenInclude(x => x.TopicArea)
            .ThenInclude(x => x!.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations)
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
    

    // see peaks vist DAL objekt olema tegelt?!
    // need HasTopicArea-d oleks mpaitud juba TopicArea
    public override async Task<IEnumerable<App.Domain.News>> AllAsync()
    {
        return await DbSet
            .Include(x => x.HasTopicAreas)
                .ThenInclude(x => x.TopicArea)
                    .ThenInclude(x => x!.LanguageString)
                        .ThenInclude(x => x!.LanguageStringTranslations)
            .Include(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                    .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .ToListAsync();
    }
    
}