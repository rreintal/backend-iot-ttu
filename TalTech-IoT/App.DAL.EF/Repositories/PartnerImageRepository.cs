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

    public override PartnerImage Add(PartnerImage entity)
    {
        if (entity.ImageResources != null)
        {
            var a = 1;
        }
        return base.Add(entity);
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

        if (entity.ImageResources != null)
        {
            foreach (var ImageResource in entity.ImageResources)
            {
                DbContext.Entry(new ImageResource()
                {
                    PartnerImageId = id,
                    Link = ImageResource.Link
                }).State = EntityState.Deleted;
            }
        }
        return await base.RemoveAsync(id);
    }
}