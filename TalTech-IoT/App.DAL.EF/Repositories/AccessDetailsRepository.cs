using App.DAL.Contracts;
using AutoMapper;
using Base.DAL.EF;
using AccessDetails = DAL.DTO.V1.AccessDetails;

namespace App.DAL.EF.Repositories;

public class AccessDetailsRepository : EFBaseRepository<App.Domain.AccessDetails, AppDbContext>, IAccessDetailsRepository
{
    public AccessDetailsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }
}