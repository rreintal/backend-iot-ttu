using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IFeedPageRepository : IBaseTranslateableRepository<FeedPage>
{
    public Task<FeedPage?> FindAsyncByName(string identifier);
    public Task<FeedPage?> FindAsyncByNameTranslated(string identifier, string languageCulture);
}