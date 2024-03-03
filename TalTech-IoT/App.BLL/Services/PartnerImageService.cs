using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;

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
        _imageStorageService.ProccessSave(entity);
        return base.Add(entity);
    }
}