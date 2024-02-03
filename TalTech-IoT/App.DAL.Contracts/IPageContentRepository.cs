using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IPageContentRepository : IBaseRepository<App.Domain.PageContent>, IPageContentRepositoryCustom<App.Domain.PageContent>
{
    
}

public interface IPageContentRepositoryCustom<TEntity>
{
    public Task<TEntity?> FindAsyncByIdentifierString(string identifier);
    public Task<TEntity?> FindAsyncByIdentifierString(string identifier, string languageCulture);

}