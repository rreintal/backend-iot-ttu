using Base.DAL.EF.Contracts;
using BLL.DTO.V1;
using Microsoft.AspNetCore.Mvc;
using PageContent = DAL.DTO.V1.PageContent;

namespace App.DAL.Contracts;

public interface IPageContentRepository : IBaseRepository<App.Domain.PageContent>, IPageContentRepositoryCustom<App.Domain.PageContent>
{
    public Task<App.Domain.PageContent?> FindAsyncByIdentifierString(string identifier);
    public Task<App.Domain.PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture); 
    
    public Task<App.Domain.PageContent?> UpdateAsync(App.Domain.PageContent entity);
}

public interface IPageContentRepositoryCustom<TEntity>
{
}