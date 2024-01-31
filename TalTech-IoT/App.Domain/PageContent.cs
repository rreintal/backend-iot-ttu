using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class PageContent : DomainEntityId, IContentEntity
{
    public string PageIdentifier { get; set; } = default!;
    
    public ICollection<Content> Content { get; set; } = default!; // contains Body + Titile
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }
}