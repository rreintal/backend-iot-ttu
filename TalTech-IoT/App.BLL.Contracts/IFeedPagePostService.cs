using Base.BLL.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IFeedPagePostService : ITranslateableEntityService<FeedPagePost>
{
    public Task<FeedPagePost> UpdateAsync(FeedPagePost entity);
}