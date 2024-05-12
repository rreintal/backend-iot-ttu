using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class AccessDetailsService : BaseEntityService<global::BLL.DTO.V1.AccessDetails, Domain.AccessDetails, IAccessDetailsRepository>, IAccessDetailsService {
    public AccessDetailsService(IAppUOW uow, IMapper<global::BLL.DTO.V1.AccessDetails, Domain.AccessDetails> mapper) : base(uow.AccessDetailsRepository, mapper)
    {
    }
}