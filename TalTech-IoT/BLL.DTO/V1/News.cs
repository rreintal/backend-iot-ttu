using Base.Domain;

namespace BLL.DTO.V1;

public class News : DomainEntityId
{
    public List<BLL.DTO.V1.Content> Content { get; set; } = default!;
    public string Author { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

    public string GetContentValue(string contentType)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString.LanguageStringTranslations!.First();
        return result.TranslationValue;
    }
}