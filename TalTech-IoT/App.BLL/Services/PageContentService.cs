using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PageContentService : BaseEntityService<PageContent, PageContent, IPageContentRepository>, IPageContentService
{
    
    private IAppUOW Uow { get; set; }
    
    public PageContentService( IAppUOW uow, IMapper<PageContent, PageContent> mapper) : base(uow.PageContentRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<PageContent?> FindAsyncByIdentifierString(string identifier)
    {
        return await Uow.PageContentRepository.FindAsyncByIdentifierString(identifier);
    }
}