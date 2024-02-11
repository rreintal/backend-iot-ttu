using App.DAL.Contracts;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using ContentType = DAL.DTO.V1.ContentType;

namespace App.DAL.EF.Repositories;

public class FeedPagePostRepository : EFBaseRepository<FeedPagePost, AppDbContext>, IFeedPagePostRepository
{
    public FeedPagePostRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override FeedPagePost Add(FeedPagePost entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.Attach(content.ContentType);
        }
        return base.Add(entity);
    }

    public Task<IEnumerable<FeedPagePost>> AllAsync(string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public async override Task<FeedPagePost?> FindAsync(Guid id)
    {
        return await DbSet
            .Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<FeedPagePost?> FindAsync(Guid id, string? languageCulture)
    {
        return await DbSet
            .Include(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                .ThenInclude(x => x.LanguageStringTranslations)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<FeedPagePost> UpdateAsync(FeedPagePost entity)
    {
        // TODO: is it possible to set a post to new category?
        
        var existingObject = await FindAsync(entity.Id);
        UpdateContentHelper.UpdateContent(existingObject, entity);
        var result = Update(existingObject);
        return result;
    }
}