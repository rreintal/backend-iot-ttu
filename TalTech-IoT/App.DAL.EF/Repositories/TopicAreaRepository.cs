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

    public async override Task<IEnumerable<TopicArea>> AllAsync()
    {
        var res = await DbSet
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations)
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<TopicArea>> AllAsync(string? languageString)
    {
        var res = await DbSet
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageString))
            .ToListAsync();
        return res;
    }

    public Task<TopicArea?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }
    
    
    // TODO - eraldi klass/objekt, mis seda kontrollib!
    private bool TopicAreaWithThisNameExists(TopicArea entity)
    {
        // TODO : REFACTORI KERGEMAKS!! EI OLE VAJA LISTI SAADA!
        var entityTranslationValues = entity.LanguageString!.LanguageStringTranslations
            .Select(elt => elt.TranslationValue)
            .ToList();

        return DbSet
            .SelectMany(ta => ta.LanguageString!.LanguageStringTranslations!)
            .Select(lst => lst.TranslationValue)
            .Any(lstValue => entityTranslationValues.Contains(lstValue));
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

    public async Task<IEnumerable<HasTopicArea>> GetHasTopicArea(TopicAreaCountFilter filter, string languageCulture)
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