using App.Domain;
using Base.Domain;

namespace DAL.DTO.V1;

public class UpdateHomePageBanner : DomainEntityId
{
    public List<DAL.DTO.V1.ContentDto> Title { get; set; } = default!;
    public List<DAL.DTO.V1.ContentDto> Body { get; set; } = default!;
    public string? Image { get; set; }
    
    public string GetContentValue(string contentType, string languageCulture)
    {
        if (contentType == ContentTypes.BODY)
        {
            return Body.Where(x => x.Culture == languageCulture).First().Value;
        }
        
        if (contentType == ContentTypes.TITLE)
        {
            return Title.Where(x => x.Culture == languageCulture).First().Value;
        }
        return "INAVLID CONTENT TYPE!";
    }
}