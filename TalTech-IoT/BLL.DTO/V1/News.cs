using Base.Domain;
using Contracts;

namespace BLL.DTO.V1;

public class News : DomainEntityId, IContainsContent
{
    public List<BLL.DTO.V1.Content> Content { get; set; } = default!;
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public string Author { get; set; } = default!;
    public string Image { get; set; } = default!;

    public string ThumbnailImage { get; set; } = default!;

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