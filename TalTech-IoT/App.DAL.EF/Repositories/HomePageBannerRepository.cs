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

    public override HomePageBanner Remove(HomePageBanner entity)
    {
        DbSet.Entry(entity).State = EntityState.Deleted;
        return base.Remove(entity);
    }

    public override async Task<IEnumerable<HomePageBanner>> AllAsync()
    {
        return await DbSet
            .IncludeContentWithTranslation()
            .OrderBy(x => x.SequenceNumber)
            .ToListAsync();
    }

    public override Task<HomePageBanner?> FindAsync(Guid id)
    {
        return DbSet
            .Where(x => x.Id == id)
            .Include(e => e.ImageResources)
            .IncludeContentWithTranslation().FirstOrDefaultAsync();
    } 
    
    public async Task<HomePageBanner?> FindAsyncTracking(Guid Id)
    {
        return await DbSet
            .AsTracking()
            .Where(entity => entity.Id == Id)
            .Include(e => e.ImageResources)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<HomePageBanner>> AllAsync(string? languageCulture)
    {
        return await DbSet
            .IncludeContentWithTranslation(languageCulture)
            .OrderBy(x => x.SequenceNumber)
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
        DbContext.Entry(existingObject.ImageResources).State = EntityState.Deleted;
        DbContext.Entry(entity.ImageResources).State = EntityState.Added;
        UpdateContentHelper.UpdateContent(existingObject, entity);
        var result = Update(existingObject);
        return result;
    }


    public async Task UpdateSequenceBulkAsync(List<HomePageBannerSequence> data)
    {
        var ids = data.Select(item => item.HomePageBannerId).ToList();
        var banners = await DbContext
            .HomePageBanners
            .AsTracking()
            .Where(banner => ids.Contains(banner.Id)).ToListAsync();

        foreach (var item in data)
        {
            var banner = banners.FirstOrDefault(b => b.Id == item.HomePageBannerId);
            if (banner != null)
            {
                banner.SequenceNumber = item.SequenceNumber;
            }
        }
        
    }
}