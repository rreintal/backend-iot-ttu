using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IPageContentRepository : IBaseRepository<App.Domain.PageContent>, IPageContentRepositoryCustom<App.Domain.PageContent>
{
    public Task<App.Domain.PageContent?> FindAsyncByIdentifierString(string identifier);
    public Task<App.Domain.PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture);      
}

public interface IPageContentRepositoryCustom<TEntity>
{
}