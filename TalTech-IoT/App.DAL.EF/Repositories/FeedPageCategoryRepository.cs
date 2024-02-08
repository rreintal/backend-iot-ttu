using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;

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

    public Task<IEnumerable<FeedPageCategory>> AllAsync(string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public Task<FeedPageCategory?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }
}