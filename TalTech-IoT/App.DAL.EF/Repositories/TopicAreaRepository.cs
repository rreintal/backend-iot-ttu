using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using BLL.DTO.V1;
using DAL.DTO.V1.FilterObjects;
using Microsoft.EntityFrameworkCore;
using News = BLL.DTO.V1.News;
using TopicArea = App.Domain.TopicArea;

namespace App.DAL.EF.Repositories;

public class TopicAreaRepository : EFBaseRepository<App.Domain.TopicArea, AppDbContext>, ITopicAreaRepository
{
    public TopicAreaRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }

    public async override Task<IEnumerable<TopicArea>> AllAsync()
    {
        var res =await DbSet
            .AsTracking()
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations
                .Where(x => x.LanguageCulture == languageCulture))
            .ToListAsync();
        return res;

    }

    public async Task<IEnumerable<HasTopicArea>> GetHasTopicArea(TopicAreaCountFilter filter)
    {
        // Võiks olla eraldi repo, aga ühe meetodi jaoks, ei näe vajadust.
        
        var query = DbContext.HasTopicAreas
            .Include(x => x.TopicArea)
                .ThenInclude(x => x!.LanguageString)
                    .ThenInclude(x => x!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .Include(x => x.TopicArea)
                .ThenInclude(x => x!.ParentTopicArea)
                    .ThenInclude(x => x!.LanguageString)
                        .ThenInclude(x => x!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .AsQueryable();

        if (filter.News == true)
        {
            return await query
                .Where(x => x.NewsId != null)
                .ToListAsync();
        }

        if (filter.Projects == true)
        {
            // TODO - projects not implemented yet
        }

        return new List<HasTopicArea>();
    }
}