using App.DAL.Contracts;
using App.Domain;
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
            .Include(x => x.FeedPagePosts)
            .ThenInclude(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.FeedPagePosts)
            .ThenInclude(x => x.Content)
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
}