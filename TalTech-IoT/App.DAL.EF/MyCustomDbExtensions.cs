using App.Domain.Contracts;
using Microsoft.EntityFrameworkCore;


namespace App.DAL.EF;

public static class MyCustomDbExtensions
{
    private const string ALL_LANGUAGE_CULTURES = "allLanguageCultures";
    public static IQueryable<T> IncludeHasTopicAreasWithTranslation<T>(
        this IQueryable<T> queryable,
        string languageCulture = ALL_LANGUAGE_CULTURES)
    where T : class, IHasTopicAreaEntity
    {
        var result = queryable
            .Include(x => x.HasTopicAreas)
            .ThenInclude(x => x.TopicArea)
            .ThenInclude(x => x!.LanguageString);

        if (languageCulture == ALL_LANGUAGE_CULTURES)
        {
            result.ThenInclude(x => x!.LanguageStringTranslations);
        }
        else
        {
            result.ThenInclude(x => x!.LanguageStringTranslations.Where(t => t.LanguageCulture == languageCulture));
        }

        return result;
    }

    public static IQueryable<T> IncludeContentWithTranslation<T>(
        this IQueryable<T> queryable, 
        string languageCulture = ALL_LANGUAGE_CULTURES)
    where T : class, IContentEntity
    {
        var result = queryable.Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString);
        
        
        if (languageCulture == ALL_LANGUAGE_CULTURES)
        {
            result.ThenInclude(x => x.LanguageStringTranslations);
        }
        else
        {
            result.ThenInclude(x => x.LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture));
        }

        return result;
    }
}