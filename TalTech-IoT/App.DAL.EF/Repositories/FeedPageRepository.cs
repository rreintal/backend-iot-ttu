using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class FeedPageRepository : EFBaseRepository<FeedPage, AppDbContext>, IFeedPageRepository
{
    public FeedPageRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public Task<IEnumerable<FeedPage>> AllAsync(string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public Task<FeedPage?> FindAsync(Guid id, string? languageCulture)
    {
        throw new NotImplementedException();
    }

    public async Task<FeedPage?> FindAsyncByName(string identifier)
    {
        return await DbSet
            .Include(x => x.FeedPageCategories)
                .ThenInclude(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.FeedPageCategories)
                .ThenInclude(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                .ThenInclude(x => x.LanguageStringTranslations)
            .Include(x => x.FeedPageCategories)
                .ThenInclude(x => x.FeedPagePosts)
                .ThenInclude(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.FeedPageCategories)
                .ThenInclude(x => x.FeedPagePosts)
                .ThenInclude(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                .ThenInclude(x => x.LanguageStringTranslations)
            .Where(e => e.FeedPageName == identifier).FirstOrDefaultAsync();
    }

    public async Task<FeedPage?> FindAsyncByNameTranslated(string identifier, string languageCulture)
    {
        return await DbSet
            .Include(x => x.FeedPageCategories)
            .ThenInclude(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.FeedPageCategories)
            .ThenInclude(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .Include(x => x.FeedPageCategories)
            .ThenInclude(x => x.FeedPagePosts)
            .ThenInclude(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.FeedPageCategories)
            .ThenInclude(x => x.FeedPagePosts)
            .ThenInclude(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .Where(e => e.FeedPageName == identifier).FirstOrDefaultAsync();
    }
}