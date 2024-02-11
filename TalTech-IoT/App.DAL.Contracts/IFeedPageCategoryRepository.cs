using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IFeedPageCategoryRepository : IBaseTranslateableRepository<FeedPageCategory>  , IFeedPageCategoryRepositoryCustom
{
    public Task<FeedPageCategory> UpdateAsync(FeedPageCategory entity);
}

public interface IFeedPageCategoryRepositoryCustom
{
    public Task<bool> DoesCategoryHavePostsAsync(Guid id);
}