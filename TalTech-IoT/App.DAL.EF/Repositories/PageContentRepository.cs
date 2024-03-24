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
        
        var cults = LanguageCulture.ALL_LANGUAGES;
        
        foreach (var lang in cults)
        {
            var oldBodyValue = existingObject.GetContentValue(ContentTypes.BODY, lang);
            var oldTitleValue = existingObject.GetContentValue(ContentTypes.TITLE, lang);
    
            var newBodyValue = entity!.GetContentValue(ContentTypes.BODY, lang);
            var newTitleValue = entity.GetContentValue(ContentTypes.TITLE, lang);

            var isBodyValueChanged = oldBodyValue != newBodyValue;
            var isTitleContentChanged = oldTitleValue != newTitleValue;

            Console.WriteLine($"Language culture: {lang}");
            Console.WriteLine($"Title: {isTitleContentChanged}");
            Console.WriteLine($"Body: {isBodyValueChanged}");
            if (isBodyValueChanged)
            {
                Console.WriteLine($"Old value: {oldBodyValue}, new value: {newBodyValue}");
                existingObject.SetContentTranslationValue(ContentTypes.BODY, lang, newBodyValue);
            }
            
            if (isTitleContentChanged)
            {
                Console.WriteLine($"Old value: {oldTitleValue}, new value: {newTitleValue}");
                existingObject.SetContentTranslationValue(ContentTypes.TITLE, lang, newTitleValue);
            }
        }

        var updatedObject = Update(existingObject);
        return updatedObject;
    }
}