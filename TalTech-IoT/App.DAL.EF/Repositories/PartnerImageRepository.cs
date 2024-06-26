using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PartnerImageRepository : EFBaseRepository<PartnerImage, AppDbContext>, IPartnerImageRepository
{
    public PartnerImageRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async override Task<PartnerImage?> FindAsync(Guid id)
    {
        return await DbSet
            .Include(e => e.ImageResources)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async override Task<PartnerImage?> RemoveAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        return await base.RemoveAsync(id);
    }
}