using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class Project : DomainEntityIdMetaData, IDomainContentEntity
{
    public int Year { get; set; } 
    public double ProjectVolume { get; set; } 
    public string ProjectManager { get; set; } = default!;

    // title, content
    public ICollection<Content> Content { get; set; } = default!;
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }

    public void SetContentTranslationValue(string contentType, string languageCulture, string value)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First();
        
        result.TranslationValue = value;
    }

    public void SetBaseLanguage(string contentType, string value)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString;
        result.Value = value;
    }
}