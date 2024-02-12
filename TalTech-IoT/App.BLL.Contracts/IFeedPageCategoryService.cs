using App.DAL.Contracts;
using Base.BLL.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IFeedPageCategoryService : ITranslateableEntityService<FeedPageCategory>, IFeedPageCategoryRepositoryCustom
{
    public Task<FeedPageCategory> UpdateAsync(FeedPageCategory entity);
    public Task<List<global::BLL.DTO.V1.FeedPageCategory>> GetCategoryWithoutPosts();
}