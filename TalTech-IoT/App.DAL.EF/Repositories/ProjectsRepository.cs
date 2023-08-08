using App.DAL.Contracts;
using App.Domain;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProjectsRepository : EFBaseRepository<App.Domain.Project, AppDbContext>, IProjectsRepository
{
    public ProjectsRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async override Task<IEnumerable<Project>> AllAsync()
    {
        var res = await DbSet
            .Include(x => x.HasTopicAreas)
                .ThenInclude(x => x.TopicArea)
                    .ThenInclude(x => x!.LanguageString)
                        .ThenInclude(x => x!.LanguageStringTranslations)
            .Include(x => x.Content)
                .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                    .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture))
            .ToListAsync();
        return res;
    }
}