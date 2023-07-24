using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TopicAreaRepository : EFBaseRepository<App.Domain.TopicArea, AppDbContext>, ITopicAreaRepository
{
    public TopicAreaRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }

    public async override Task<IEnumerable<TopicArea>> AllAsync()
    {
        return await DbSet
            .Include(x => x.LanguageString)
            .ThenInclude(x => x!.LanguageStringTranslations
                .Where(x => x.LanguageCulture == languageCulture))
            .ToListAsync();

    }
}