using App.Domain.Contracts;
using Base.Domain;
using Contracts;

namespace App.Domain;

public class News : DomainEntityIdMetaData, IHasTopicAreaEntity, IDomainContentEntity
{
    public ICollection<Content> Content { get; set; } = default!;
    public ICollection<HasTopicArea> HasTopicAreas { get; set; } = default!;

    public string? Image { get; set; }
    public string? ThumbnailImage;
    public string Author { get; set; } = default!;

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