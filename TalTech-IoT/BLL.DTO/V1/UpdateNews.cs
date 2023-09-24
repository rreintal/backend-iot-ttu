using App.Domain;
using Base.Domain;

namespace BLL.DTO.V1;

public class UpdateNews : DomainEntityId
{
    public string Author { get; set; } = default!;
    public List<BLL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;
    public List<BLL.DTO.V1.RawContent> Title { get; set; } = default!;
    public List<BLL.DTO.V1.RawContent> Body { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; } = default!;

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

        // TODO - trhow an exception
        return "INAVLID CONTENT TYPE!";
    }
}