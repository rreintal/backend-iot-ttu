using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class HomePageBannerRepository : EFBaseRepository<HomePageBanner, AppDbContext>, IHomePageBannerRepository
{
    public HomePageBannerRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override HomePageBanner Add(HomePageBanner entity)
    {
        foreach (var content in entity.Content)
        {
            // Doing this database does not try to add again types to db.
            DbContext.Attach(content.ContentType);
        }
        
        return base.Add(entity);
    }

    public override async Task<IEnumerable<HomePageBanner>> AllAsync()
    {
        return await DbSet.IncludeContentWithTranslation().ToListAsync();
    }

    public override Task<HomePageBanner?> FindAsync(Guid id)
    {
        return DbSet.Where(x => x.Id == id).IncludeContentWithTranslation().FirstOrDefaultAsync();
    } 

    public async Task<IEnumerable<HomePageBanner>> AllAsync(string? languageCulture)
    {
        return await DbSet
            .IncludeContentWithTranslation(languageCulture)
            .ToListAsync();
    }

    public async Task<HomePageBanner?> FindAsync(Guid id, string? languageCulture)
    {
        return await DbSet
            .Where(entity => entity.Id == id)
            .IncludeContentWithTranslation(languageCulture)
            .FirstOrDefaultAsync();
    }

    public async Task<HomePageBanner> UpdateAsync(HomePageBanner entity)
    {
        var existingObject = await FindAsync(entity.Id);

        UpdateContentHelper.UpdateContent(existingObject, entity);
        var result = Update(existingObject);
        return result;
    }


    public async Task<HomePageBanner> Update(UpdateHomePageBanner entity)
    {
        var existingDomainObject = await FindAsync(entity.Id);

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
        var result = Update(existingDomainObject);
        return result;
    }
}