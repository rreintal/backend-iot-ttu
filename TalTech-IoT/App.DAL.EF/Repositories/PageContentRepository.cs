using App.DAL.Contracts;
using App.DAL.EF.DbExtensions;
using App.Domain;
using App.Domain.Helpers;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PageContentRepository : EFBaseRepository<App.Domain.PageContent, AppDbContext>, IPageContentRepository
{
    public PageContentRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public override PageContent Add(PageContent entity)
    {
        foreach (var content in entity.Content)
        {
            DbContext.Attach(content.ContentType);
        }
        
        return base.Add(entity);
    }

    public async Task<PageContent?> FindAsyncByIdentifierString(string identifier)
    {
        return await DbSet
            .Where(e => e.PageIdentifier == identifier)
            .Include(e => e.ImageResources)
            .IncludeContentWithTranslation()
            .FirstOrDefaultAsync();
    }


    public async Task<PageContent?> FindAsyncByIdentifierString(string identifier, string languageCulture)
    {
        return await DbSet
            .AsNoTracking()
            .Where(e => e.PageIdentifier == identifier)
            .IncludeContentWithTranslation(languageCulture)
            .FirstOrDefaultAsync();
    }

    public async Task<PageContent?> UpdateAsync(PageContent entity)
    {
        var existingObject = await FindAsyncByIdentifierString(entity.PageIdentifier);
        if (existingObject == null)
        {
            return null;
        }

        if (entity.ImageResources != null)
        {
            if (existingObject.ImageResources != null)
            {
                DbContext.ImageResources.RemoveRange(existingObject.ImageResources);

                foreach (var imageResource in entity.ImageResources)
                {
                    var item = new ImageResource()
                    {
                        PageContentId = existingObject.Id,
                        Link = imageResource.Link
                    };

                    DbContext.Entry(item).State = EntityState.Added;
                }
            }
            else
            {
                existingObject.ImageResources = new List<ImageResource>();
                foreach (var imageResource in entity.ImageResources)
                {
                    var item = new ImageResource()
                    {
                        PageContentId = existingObject.Id,
                        Link = imageResource.Link
                    };
                    DbContext.Entry(item).State = EntityState.Added;
                }
            }
        }
        
        UpdateContentHelper.UpdateContent(existingObject, entity);
        var updatedObject = Update(existingObject);
        return updatedObject;
    }
}