using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PartnerImageRepository : EFBaseRepository<PartnerImage, AppDbContext>, IPartnerImageRepository
{
    public PartnerImageRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }
}