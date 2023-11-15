using App.DAL.Contracts;
using App.DAL.EF.DbExceptions;
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

    // TODO - eraldi klass/objekt, mis seda kontrollib!
    private bool TopicAreaWithThisNameExists(TopicArea entity)
    {
        return DbSet.Any(ta => ta.LanguageString!.Value == entity.LanguageString!.Value);
    }

    
    
    public override TopicArea Add(TopicArea entity)
    {
        // TODO - tee mingi index, value pealt äkki ja kasuta seda?
        if (TopicAreaWithThisNameExists(entity))
        {
            throw new DbValidationExceptions()
            {
                ErrorMessage = "NAME_ALREADY_EXISTS",
                ErrorCode = 400
            };
        }
        return base.Add(entity);
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

    public async Task<IEnumerable<TopicArea>> GetTopicAreasWithAllTranslations()
    {
        var res = await DbSet
            .AsTracking()
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations)
            .ToListAsync();
        return res;
    }
}