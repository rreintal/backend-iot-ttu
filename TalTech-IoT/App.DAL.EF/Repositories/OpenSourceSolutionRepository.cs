using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OpenSourceSolution = DAL.DTO.V1.OpenSourceSolution;

namespace App.DAL.EF.Repositories;

public class OpenSourceSolutionRepository : EFBaseRepository<Domain.OpenSourceSolution, AppDbContext>, IOpenSourceSolutionRepository
{
    public OpenSourceSolutionRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override Domain.OpenSourceSolution Add(Domain.OpenSourceSolution entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.Attach(content.ContentType);
        }

        entity.CreatedAt = DateTime.UtcNow;
        return base.Add(entity);
    }

    public async Task<IEnumerable<Domain.OpenSourceSolution>> AllAsync(string? languageCulture)
    {
        return await DbSet
            .IncludeContentWithTranslation(languageCulture)
            .ToListAsync();
    }

    public async Task<Domain.OpenSourceSolution?> FindAsync(Guid id, string? languageCulture)
    {
        return await DbSet.Where(x => x.Id == id)
            .IncludeContentWithTranslation(languageCulture)
            .FirstOrDefaultAsync();
    }

    public async override Task<Domain.OpenSourceSolution?> FindAsync(Guid id)
    {
        return await DbSet.Where(x => x.Id == id)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }

    public override Domain.OpenSourceSolution? Find(Guid id)
    {
        return DbSet.Where(x => x.Id == id)
            .AsTracking()
            .IncludeContentWithTranslation()
            .FirstOrDefault();
    }

    public  override Domain.OpenSourceSolution Update(Domain.OpenSourceSolution entity)
    {
        // TODO: Create UpdateAsync
        var existingEntity = Find(entity.Id);
        UpdateContentHelper.UpdateContent(existingEntity, entity);
        existingEntity.Private = entity.Private;
        existingEntity.Link = entity.Link;
        return base.Update(existingEntity);
    }

    public override Domain.OpenSourceSolution Remove(Domain.OpenSourceSolution entity)
    {
        // TODO: wtf?
        DbContext.Entry(entity).State = EntityState.Deleted;
        return base.Remove(entity);
    }

    public async Task<int> GetCount()
    {
        return await DbSet.CountAsync();
    }
}