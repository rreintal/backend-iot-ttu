using Base.Domain;
using Contracts;

namespace BLL.DTO.V1;

public class Project : DomainEntityId, IContainsContent
{
    public int Year { get; set; } = default!;
    public string ProjectManager { get; set; } = default!;
    public double ProjectVolume { get; set; } = default!;
    public string Image { get; set; } = default!;

    public string ThumbnailImage { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;

    // body + title
    public List<Content> Content { get; set; } = default!;
    
    public string GetContentValue(string contentType)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString.LanguageStringTranslations!.First();
        return result.TranslationValue;
    }
    
}