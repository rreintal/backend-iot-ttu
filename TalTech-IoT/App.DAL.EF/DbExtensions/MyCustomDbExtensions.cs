using App.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.DbExtensions;

public static class MyCustomDbExtensions
{
    private const string ALL_LANGUAGE_CULTURES = "allLanguageCultures";
    public static IQueryable<T> IncludeHasTopicAreasWithTranslation<T>(
        this IQueryable<T> queryable,
        string languageCulture = ALL_LANGUAGE_CULTURES)
    where T : class, IHasTopicAreaEntity
    {
        if (languageCulture == ALL_LANGUAGE_CULTURES)
        {
            var resultWithAllLanguages = queryable
                .Include(x => x.HasTopicAreas)
                .ThenInclude(x => x.TopicArea)
                .ThenInclude(x => x!.LanguageString)
                .ThenInclude(x => x!.LanguageStringTranslations);
            return resultWithAllLanguages;
        }
        var result = queryable
                .Include(x => x.HasTopicAreas)
                .ThenInclude(x => x.TopicArea)
                .ThenInclude(x => x!.LanguageString)
                .ThenInclude(x => x!.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture));
        
        

        return result;
    }

    public static IQueryable<T> IncludeContentWithTranslation<T>(
        this IQueryable<T> queryable, 
        string languageCulture = ALL_LANGUAGE_CULTURES)
    where T : class, IContentEntity
    {
        if (languageCulture == ALL_LANGUAGE_CULTURES)
        {
            var resultWithAllLanguages =
                queryable
                    .Include(x => x.Content)
                    .ThenInclude(x => x.ContentType)
                    .Include(x => x.Content)
                    .ThenInclude(x => x.LanguageString)
                    .ThenInclude(x => x.LanguageStringTranslations);
            return resultWithAllLanguages;
        }
        var result =
            queryable
                .Include(x => x.Content)
                .ThenInclude(x => x.ContentType)
                .Include(x => x.Content)
                .ThenInclude(x => x.LanguageString)
                .ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture));
        return result;
    }
}