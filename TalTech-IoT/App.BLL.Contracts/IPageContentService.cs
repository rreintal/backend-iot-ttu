using App.DAL.Contracts;
using Base.BLL.Contracts;
using BLL.DTO.V1;
using Microsoft.AspNetCore.Mvc;

namespace App.BLL.Contracts;

public interface IPageContentService : IEntityService<PageContent>, IPageContentRepositoryCustom<App.Domain.PageContent>
{
    public Task<PageContent?> FindAsyncByIdentifierString(string identifier);
    public Task<PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture);

    public Task<global::BLL.DTO.V1.PageContent?> UpdateAsync(PageContent entity);
}