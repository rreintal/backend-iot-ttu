using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1;
using ImageResource = BLL.DTO.V1.ImageResource;

namespace App.BLL.Services;

public class HomePageBannerService : BaseEntityService<HomePageBanner, Domain.HomePageBanner, IHomePageBannerRepository>, IHomePageBannerService
{
    private IAppUOW _uow;
    private IMapper _mapper;
    private IImageStorageService _imageStorageService { get; set; }

    public HomePageBannerService(IAppUOW uow, IMapper<HomePageBanner, Domain.HomePageBanner> mapper, IMapper autoMapper) : base(uow.HomePageBannerRepository, mapper)
    {
        _uow = uow;
        _mapper = autoMapper;
        _imageStorageService = new ImageStorageService.ImageStorageService();
    }
    // TODO: here do CDN magic!
    public async Task<IEnumerable<HomePageBanner>> AllAsync(string? languageCulture)
    {
        return (await _uow.HomePageBannerRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }
    public override HomePageBanner Add(HomePageBanner entity)
    {
        var imageResources = _imageStorageService.ProccessSave(entity);
        if(imageResources != null && imageResources.SavedLinks != null && imageResources.SavedLinks.Count == 1)
        {
            var item = new ImageResource()
            {
                HomePageBannerId = entity.Id,
                Link = imageResources.SavedLinks[0]
            };
            entity.ImageResources = item;
        }
        return base.Add(entity);
    }

    public async Task<HomePageBanner?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.HomePageBannerRepository.FindAsync(id, languageCulture));
    }

    public async override Task<HomePageBanner?> RemoveAsync(Guid id)
    {
        var entity = await _uow.HomePageBannerRepository.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        if (entity.ImageResources != null)
        {
            var deleteContent = new DeleteContent()
            {
                Links = new List<string>() {entity.ImageResources.Link}
            };
            _imageStorageService.ProcessDelete(deleteContent);
        }
        
        
        return await base.RemoveAsync(id);
    }
    
    public async Task<HomePageBanner?> UpdateAsync(HomePageBanner entity)
    {
        var existing =  await _uow.HomePageBannerRepository.FindAsync(entity.Id);
        if (existing == null || existing.ImageResources == null)
        {
            return null;
        }
        var domainObject = _mapper.Map<Domain.HomePageBanner>(entity);

        entity.ImageResources = new ImageResource()
        {
            HomePageBannerId = entity.Id,
            Link = existing.ImageResources.Link
        };
        var updateResult = _imageStorageService.ProccessUpdate(entity);
        if (updateResult != null && updateResult.DeletedLinks != null)
        {
            var deleteContent = new DeleteContent()
            {
                Links = updateResult.DeletedLinks
            };
            _imageStorageService.ProcessDelete(deleteContent);
        }

        if (updateResult != null && updateResult.SavedLinks != null)
        {
            var item = updateResult.SavedLinks[0];
            domainObject.ImageResources = new Domain.ImageResource()
            {
                HomePageBannerId = domainObject.Id,
                Link = item
            };
            domainObject.Image = item;
        }
        
        
        
        var result = await _uow.HomePageBannerRepository.UpdateAsync(domainObject);
        return _mapper.Map<HomePageBanner>(result);
    }
    

    public async Task UpdateSequenceBulkAsync(List<HomePageBannerSequence> data)
    {
        await _uow.HomePageBannerRepository.UpdateSequenceBulkAsync(data);
    }
}