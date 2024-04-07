using Base.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;
using Contracts;

namespace BLL.DTO.V1;

public class Project : DomainEntityId, IContentEntity, IContainsImageResource
{
    public int Year { get; set; } = default!;
    public string ProjectManager { get; set; } = default!;
    public double ProjectVolume { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsOngoing { get; set; }
    public List<Content> Content { get; set; } = default!;

    public int ViewCount { get; set; }
    public List<ImageResource> ImageResources { get; set; } = new List<ImageResource>();
    
    public string GetContentValue(string contentType)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString.LanguageStringTranslations!.First();
        return result.TranslationValue;
    }
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }
    
}