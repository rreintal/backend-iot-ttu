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
        var maxSequenceNumber = DbContext.HomePageBanners.Max(banner => (int?)banner.SequenceNumber) ?? 0;

        entity.SequenceNumber = maxSequenceNumber + 1;

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
        existingObject!.Image = entity.Image;
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

    public async Task UpdateSequenceBulkAsync(List<HomePageBannerSequence> data)
    {
        var ids = data.Select(item => item.HomePageBannerId).ToList();
        var banners = await DbContext.HomePageBanners.Where(banner => ids.Contains(banner.Id)).ToListAsync();

        foreach (var item in data)
        {
            var banner = banners.FirstOrDefault(b => b.Id == item.HomePageBannerId);
            if (banner != null)
            {
                banner.SequenceNumber = item.SequenceNumber;
                Update(banner);
            }
            else
            {
                
                // TODO: handle case when banner with this ID was not found, return error
            }
        }
        
    }
}