using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class FeedPagePostRepository : EFBaseRepository<FeedPagePost, AppDbContext>, IFeedPagePostRepository
{
    public FeedPagePostRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public Task<IEnumerable<FeedPagePost>> AllAsync(string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public Task<FeedPagePost?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }
}