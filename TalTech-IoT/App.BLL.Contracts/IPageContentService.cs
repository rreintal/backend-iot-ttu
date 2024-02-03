using App.DAL.Contracts;
using Base.BLL.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IPageContentService : IEntityService<PageContent> // IPageContentRepositoryCustom<App.Domain.PageContent>
{
    public Task<PageContent?> FindAsyncByIdentifierString(string identifier);
    public Task<PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture);
}