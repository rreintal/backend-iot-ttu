using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using ImageResource = BLL.DTO.V1.ImageResource;

namespace App.BLL.Services;

public class PartnerImageService : BaseEntityService<global::BLL.DTO.V1.PartnerImage, PartnerImage, IPartnerImageRepository>, IPartnerImageService
{
    private IAppUOW Uow { get; }
    private IImageStorageService _imageStorageService { get; }
    public PartnerImageService(IAppUOW uow, IMapper<global::BLL.DTO.V1.PartnerImage, PartnerImage> mapper) : base(uow.PartnerImageRepository, mapper)
    {
        Uow = uow;
        _imageStorageService = new ImageStorageService.ImageStorageService();
    }

    public override global::BLL.DTO.V1.PartnerImage Add(global::BLL.DTO.V1.PartnerImage entity)
    {
        var saveResult = _imageStorageService.ProccessSave(entity);
        if (saveResult != null && saveResult.SavedLinks != null)
        {
            entity.ImageResources = saveResult.SavedLinks.Select(e => new ImageResource()
            {
                PartnerImageId = entity.Id,
                Link = e
            }).ToList();
        }
        
        return base.Add(entity);
    }

    public Task<global::BLL.DTO.V1.PartnerImage?> UpdateAsync(global::BLL.DTO.V1.PartnerImage entity)
    {
        throw new NotImplementedException();
    }

    public async override Task<global::BLL.DTO.V1.PartnerImage?> RemoveAsync(Guid id)
    {
        var existingEntity = await Uow.PartnerImageRepository.FindAsync(id);
        if (existingEntity == null)
        {
            return null;
        }

        if (existingEntity.ImageResources != null)
        {
            var deleteContent = new DeleteContent()
            {
                Links = new List<string>()
            };
            foreach (var imageResource in existingEntity.ImageResources)
            {
                deleteContent.Links.Add(imageResource.Link);
            }

            _imageStorageService.ProcessDelete(deleteContent);
        }
        return await base.RemoveAsync(id);
    }
}