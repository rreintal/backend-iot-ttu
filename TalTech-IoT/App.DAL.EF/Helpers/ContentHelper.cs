using App.Domain;
using App.Domain.Contracts;

namespace App.DAL.EF.Helpers;

public class ContentHelper
{
    public static string? GetContentValue(IDomainContentEntity entity, string contentType, string languageCulture)
    {
        var result = entity.Content
            .First(content => content.ContentType?.Name == contentType)
            .LanguageString?.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        if (result == null)
        {
            Console.WriteLine($"ContentHelper (Domain): for entity with type {entity.GetType()} could not get content value.");
        }
        return result;
    }

    public static bool CheckIfContentHasChanged(IDomainContentEntity domainEntity, IDALContentEntity dalEntity)
    {
        return false;
    }
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        // TODO - trhow an exception
        return "INAVLID CONTENT TYPE!";
    }
}