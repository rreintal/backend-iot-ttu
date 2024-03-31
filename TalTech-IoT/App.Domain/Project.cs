using System.ComponentModel.DataAnnotations;
using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class Project : DomainEntityIdMetaData, IContentEntity
{
    public int Year { get; set; } 
    public double ProjectVolume { get; set; } 
    public string ProjectManager { get; set; } = default!;

    public bool IsOngoing { get; set; }
    
    public ICollection<ImageResource>? ImageResources { get; set; }

    // title, content
    public ICollection<Content> Content { get; set; } = default!;

    public int ViewCount { get; set; }
    
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
}