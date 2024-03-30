using App.DAL.Contracts;
using App.DAL.EF.DbExceptions;
using AutoMapper;
using Base.DAL.EF;
using BLL.DTO.V1;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using TopicArea = App.Domain.TopicArea;

namespace App.DAL.EF.Repositories;

public class TopicAreaRepository : EFBaseRepository<App.Domain.TopicArea, AppDbContext>, ITopicAreaRepository
{
    public TopicAreaRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }

    public override async Task<IEnumerable<TopicArea>> AllAsync()
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

    public async Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(string languageCulture)
    {
        var topicAreasWithCount = await DbContext.TopicAreas
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .Select(ta => new 
            { 
                TopicArea = ta, 
                Count = DbContext.HasTopicAreas
                    .Count(hta => hta.TopicAreaId == ta.Id && hta.NewsId.HasValue) 
            })
            .Select(ta => new TopicAreaWithCount
            {
                Id = ta.TopicArea.Id,
                Name = ta!.TopicArea!.LanguageString!.LanguageStringTranslations
                    .FirstOrDefault(lst => lst.LanguageCulture == languageCulture)!.TranslationValue,
                Count = ta.Count
            })
            .ToListAsync();

        return topicAreasWithCount;
    }

    public override TopicArea Remove(TopicArea entity)
    {
        var hasAssociatedNews = DbContext.HasTopicAreas.Count(e => e.TopicAreaId == entity.Id);
        if (hasAssociatedNews > 0)
        {
            throw new TopicAreaDeleteConstraintViolationException();
        }
        return base.Remove(entity);
    }
}