using App.Domain;
using Base.DAL.EF.Contracts;
using AccessDetails = BLL.DTO.V1.AccessDetails;

namespace App.DAL.Contracts;

public interface IAccessDetailsRepository : IBaseRepository<App.Domain.AccessDetails>
{
    
}