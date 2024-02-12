using Base.DAL.EF.Contracts;
using BLL.DTO.V1;
using FeedPageCategory = App.Domain.FeedPageCategory;

namespace App.DAL.Contracts;

public interface IFeedPageCategoryRepository : IBaseTranslateableRepository<FeedPageCategory>  , IFeedPageCategoryRepositoryCustom
{
    public Task<FeedPageCategory> UpdateAsync(FeedPageCategory entity);
    public Task<List<FeedPageCategory>> GetFeedPageCategoryWithoutPosts();
}

public interface IFeedPageCategoryRepositoryCustom
{
    public Task<bool> DoesCategoryHavePostsAsync(Guid id);
}