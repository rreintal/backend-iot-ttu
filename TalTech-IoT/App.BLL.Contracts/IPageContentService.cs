using App.DAL.Contracts;
using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IPageContentService : IEntityService<App.Domain.PageContent>, IPageContentRepositoryCustom<App.Domain.PageContent>
{
    
}