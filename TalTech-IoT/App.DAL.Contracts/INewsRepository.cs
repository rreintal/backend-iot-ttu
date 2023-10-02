using Base.DAL.EF.Contracts;
using DAL.DTO.V1;
using DAL.DTO.V1.FilterObjects;

namespace App.DAL.Contracts;

public interface INewsRepository : IBaseRepository<App.Domain.News>, INewsRepositoryCustom<App.Domain.News>
{
    // here methods for only repo!
    // DAL objects!!!
    //TEntity Add(TEntity entity);
    public Task<IEnumerable<App.Domain.News>> AllAsyncFiltered(int? page, int? size);

    public Task<App.Domain.News?> FindByIdWithAllTranslationsAsync(Guid Id);

    public Task<App.Domain.News> Update(UpdateNews entity);
}

public interface INewsRepositoryCustom<TEntity>
{
    // here methods which are shared between repo and service!
}