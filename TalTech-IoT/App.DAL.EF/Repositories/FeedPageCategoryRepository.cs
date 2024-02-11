using App.DAL.Contracts;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class FeedPageCategoryRepository : EFBaseRepository<FeedPageCategory, AppDbContext>, IFeedPageCategoryRepository
{
    public FeedPageCategoryRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override FeedPageCategory Add(FeedPageCategory entity)
    {
        foreach (var type in entity.Content)
        {
            DbContext.Attach(type.ContentType);
        }
        
        return base.Add(entity);
    }

    public async override Task<FeedPageCategory?> FindAsync(Guid id)
    {
        return await DbSet
            .AsTracking()
            .Include(x => x.FeedPagePosts)
                .ThenInclude(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                .ThenInclude(x => x.LanguageStringTranslations)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task<IEnumerable<FeedPageCategory>> AllAsync(string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public Task<FeedPageCategory?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesCategoryHavePostsAsync(Guid id)
    {
        var count = await DbSet.Where(e => e.Id == id)
            .Where(e => e.FeedPagePosts != null && e.FeedPagePosts.Count > 0)
            .CountAsync();
        return count > 0;
    }

    public async Task<FeedPageCategory> UpdateAsync(FeedPageCategory entity)
    {
        var existingObject = await FindAsync(entity.Id);
        Console.WriteLine(entity.Id);
        UpdateContentHelper.UpdateContent(existingObject, entity, isOnlyTitleEntity: true);
        var result = Update(existingObject);
        return result;
    }
}