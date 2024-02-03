using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PageContentRepository : EFBaseRepository<App.Domain.PageContent, AppDbContext>, IPageContentRepository
{
    public PageContentRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async Task<PageContent?> FindAsyncByIdentifierString(string identifier)
    {
        return await DbSet
            .Where(e => e.PageIdentifier == identifier)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }
}