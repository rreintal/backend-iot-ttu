using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace App.BLL.Services;

public class PageContentService : BaseEntityService<global::BLL.DTO.V1.PageContent, PageContent, IPageContentRepository>, IPageContentService
{
    
    private IAppUOW Uow { get; set; }
    
    public PageContentService( IAppUOW uow, IMapper<global::BLL.DTO.V1.PageContent, PageContent> mapper) : base(uow.PageContentRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<global::BLL.DTO.V1.PageContent?> FindAsyncByIdentifierString(string identifier)
    {
        var domainEntity = await Uow.PageContentRepository.FindAsyncByIdentifierString(identifier);
        var result = Mapper.Map(domainEntity);
        return result;
    }

    public async Task<global::BLL.DTO.V1.PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture)
    {
        var domainEntity = await Uow.PageContentRepository.FindAsyncByIdentifierString(identifier, languageCulture);
        var result = Mapper.Map(domainEntity);
        return result;
    }

    public async Task<global::BLL.DTO.V1.PageContent?> UpdateAsync(global::BLL.DTO.V1.PageContent entity)
    {
        var domainEntity = Mapper.Map(entity);
        var updatedDomainEntity = await Uow.PageContentRepository.UpdateAsync(domainEntity);
        return Mapper.Map(updatedDomainEntity);
    }

    public PageContent Update(PageContent entity)
    {
        throw new NotImplementedException();
    }
}