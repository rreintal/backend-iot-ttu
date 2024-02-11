using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IFeedPagePostRepository : IBaseTranslateableRepository<FeedPagePost>
{
    public Task<FeedPagePost> UpdateAsync(FeedPagePost entity);
}