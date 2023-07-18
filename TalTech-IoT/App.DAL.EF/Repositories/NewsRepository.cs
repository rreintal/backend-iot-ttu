using App.DAL.Contracts;
using AutoMapper;
using Base.DAL.EF;
using DAL.DTO.V1;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class NewsRepository : EFBaseRepository<App.Domain.News, AppDbContext>, INewsRepository
{
    public NewsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }

    public override Domain.News Add(Domain.News entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.Attach(content.ContentType);
        }

        entity.CreatedAt = DateTime.UtcNow;

        var res = DbSet.Add(entity).Entity;
        return res;
    }

    public async Task<News?> FindById(Guid id)
    {
        // TODO
        // filter based on language, to not fetch all the results!!
        
        var query = await DbSet.Where(x => x.Id == id)
            .Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations)
            .FirstOrDefaultAsync();

        if (query == null)
        {
            return null;
        }

        return _mapper.Map<News>(query);
    }
    
}