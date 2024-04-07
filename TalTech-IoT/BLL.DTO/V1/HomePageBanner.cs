using Base.Domain;
using BLL.DTO.Contracts;

namespace BLL.DTO.V1;

public class HomePageBanner : DomainEntityId, IContainsImage, IContainsOneImageResource
{

    public List<Content> Content { get; set; } = default!;
    
    public string Image { get; set; } = default!;
    public int? SequenceNumber { get; set; }

    public ImageResource? ImageResources { get; set; }
    
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