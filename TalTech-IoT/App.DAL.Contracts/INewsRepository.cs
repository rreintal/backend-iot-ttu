using Base.DAL.EF.Contracts;
using DAL.DTO.V1;

namespace App.DAL.Contracts;

public interface INewsRepository : IBaseRepository<App.Domain.News>, INewsRepositoryCustom<App.Domain.News>
{
    // here methods for only repo!
    //TEntity Add(TEntity entity);
    public Task<News?> FindById(Guid id);
}

public interface INewsRepositoryCustom<TEntity>
{
    // here methods which are shared between repo and service!
}