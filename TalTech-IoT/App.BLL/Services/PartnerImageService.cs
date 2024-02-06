using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PartnerImageService : BaseEntityService<global::BLL.DTO.V1.PartnerImage, PartnerImage, IPartnerImageRepository>, IPartnerImageService
{
    private IAppUOW Uow { get; }
    public PartnerImageService(IAppUOW uow, IMapper<global::BLL.DTO.V1.PartnerImage, PartnerImage> mapper) : base(uow.PartnerImageRepository, mapper)
    {
        Uow = uow;
    }
}