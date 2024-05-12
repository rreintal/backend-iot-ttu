using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using ImageResource = BLL.DTO.V1.ImageResource;

namespace App.BLL.Services;

public class PageContentService : BaseEntityService<global::BLL.DTO.V1.PageContent, PageContent, IPageContentRepository>, IPageContentService
{
    
    private IAppUOW Uow { get; set; }
    
    private IImageStorageService _imageStorageService { get; }
    
    public PageContentService( IAppUOW uow, IMapper<global::BLL.DTO.V1.PageContent, PageContent> mapper) : base(uow.PageContentRepository, mapper)
    {
        Uow = uow;
        _imageStorageService = new ImageStorageService.ImageStorageService();
    }

    public override global::BLL.DTO.V1.PageContent Add(global::BLL.DTO.V1.PageContent entity)
    {
        var updateResult = _imageStorageService.ProccessSave(entity);
        if (updateResult != null && updateResult.SavedLinks != null)
        {
            entity.ImageResources = updateResult.SavedLinks.Select(e => new ImageResource()
            {
                PageContentId = entity.Id,
                Link = e
            }).ToList();
        }
        return base.Add(entity);
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
        var existing = await Uow.PageContentRepository.FindAsyncByIdentifierString(entity.PageIdentifier);
        if (existing == null)
        {
            return null;
        }

        if (existing.ImageResources != null)
        {
            entity.ImageResources = existing.ImageResources.Select(e => new ImageResource()
            {
                PageContentId = entity.Id,
                Link = e.Link
            }).ToList();
        }
        
        var updateResult = _imageStorageService.ProccessUpdate(entity);
        _imageStorageService.HandleEntityImageResources(entity, updateResult);
        var domainEntity = Mapper.Map(entity);
        var updatedDomainEntity = await Uow.PageContentRepository.UpdateAsync(domainEntity);
        return Mapper.Map(updatedDomainEntity);
    }
    public PageContent Update(PageContent entity)
    {
        throw new NotImplementedException();
    }
}