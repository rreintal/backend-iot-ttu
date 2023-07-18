using Base.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BLL.DTO.V1;

public class News : DomainEntityId
{
    public List<BLL.DTO.V1.Content> Content { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string GetContentValue(string languageCulture, string contentType)
    {
        var content = Content
            .First(item => item.ContentType!.Name == contentType)
            .LanguageString.LanguageStringTranslations!
            .First(item => item.LanguageCulture == languageCulture);
        return content.TranslationValue;
    }
}