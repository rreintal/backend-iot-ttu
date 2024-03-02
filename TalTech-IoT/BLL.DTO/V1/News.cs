using Base.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;
using Contracts;

namespace BLL.DTO.V1;

public class News : DomainEntityId, IContainsContent, IContentEntity, IContainsImage, IContainsThumbnail
{
    public List<BLL.DTO.V1.Content> Content { get; set; } = default!;
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

    public List<BLL.DTO.V1.ImageResource> ImageResources { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public string Author { get; set; } = default!;
    public string Image { get; set; } = default!;

    public string ThumbnailImage { get; set; } = default!;

    public string GetContentValue(string contentType)
    {
        try
        {
            var result = Content.First(c => c.ContentType!.Name == contentType)
                .LanguageString.LanguageStringTranslations!.First();
            return result.TranslationValue;
        }
        catch
        {
            return "ERROR IN BLL.NEWS";
        }
    }
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }
}