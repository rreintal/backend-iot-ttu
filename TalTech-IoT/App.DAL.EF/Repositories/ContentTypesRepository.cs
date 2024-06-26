using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContentTypesRepository : EFBaseRepository<App.Domain.ContentType, AppDbContext>, IContentTypesRepository
{
    public ContentTypesRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async Task<ContentType> FindByName(string name)
    {
        return await DbSet.Where(x => x.Name == name).AsNoTracking().FirstAsync();
    }
}