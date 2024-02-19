using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using App.Domain.Helpers;
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

    public override ContactPerson? Find(Guid id)
    {
        var a=  DbSet
            .IncludeContentWithTranslation()
            .FirstOrDefault(e => e.Id == id);
        return a;

    }


    public override ContactPerson Update(ContactPerson entity)
    {
        var existingObject = Find(entity.Id);
        UpdateContentHelper.UpdateContent(existingObject, entity, UpdateTitle: false);
        existingObject.Name = entity.Name;
        return base.Update(existingObject);
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