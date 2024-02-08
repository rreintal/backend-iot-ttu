using Base.BLL.Contracts;
using FeedPage = BLL.DTO.V1.FeedPage;

namespace App.BLL.Contracts;

public interface IFeedPageService : ITranslateableEntityService<FeedPage>
{
    public Task<FeedPage?> FindAsyncByName(string identifier);
}