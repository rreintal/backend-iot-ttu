using Base.DAL.EF.Contracts;
using DAL.DTO.V1;
using Public.DTO;

namespace App.DAL.Contracts;

public interface INewsRepository :IBaseTranslateableRepository<App.Domain.News>, INewsRepositoryCustom<App.Domain.News>
{
    // here methods for only repo!
    // DAL objects!!!
    public Task<IEnumerable<App.Domain.News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageString);

    public Task<App.Domain.News?> FindByIdWithAllTranslationsAsync(Guid Id);
    
    public Task<App.Domain.News?> FindByIdWithAllTranslationsAsyncNoTracking(Guid Id);

    public Task<App.Domain.News?> Update(News dalEntity);

    public News Add(News entity);

    public Domain.News? FindByIdWithAllTranslations(Guid id);

    public Task<News> AddAsync(News entity);
}

public interface INewsRepositoryCustom<TEntity>
{
    // here methods which are shared between repo and service!
    public Task<int> FindNewsTotalCount(Guid? TopicAreaId);
}