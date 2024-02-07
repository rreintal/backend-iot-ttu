using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContactPersonRepository : EFBaseRepository<ContactPerson, AppDbContext>, IContactPersonRepository
{
    public ContactPersonRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override ContactPerson Add(ContactPerson entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.Attach(content.ContentType);
        }
        return base.Add(entity);
    }

    public async Task<IEnumerable<ContactPerson>> AllAsync(string? languageCulture)
    {
        return await DbSet.IncludeContentWithTranslation(languageCulture).ToListAsync();
    }

    public async Task<ContactPerson?> FindAsync(Guid id, string? languageCulture)
    {
        return await DbSet.Where(e => e.Id == id)
            .IncludeContentWithTranslation(languageCulture)
            .FirstOrDefaultAsync();
    }

    public async override Task<ContactPerson?> FindAsync(Guid id)
    {
        return await DbSet.Where(e => e.Id == id).IncludeContentWithTranslation().FirstOrDefaultAsync();
    }
}