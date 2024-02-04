using App.Domain;
using Base.Domain;

namespace BLL.DTO.V1;

public class UpdateProject : DomainEntityId
{
    public int? Year { get; set; } = default!;
    public string? ProjectManager { get; set; }
    public double? ProjectVolume { get; set; }
    
    public List<BLL.DTO.V1.TopicArea>? TopicAreas { get; set; }
    public List<BLL.DTO.V1.ContentDto> Title { get; set; } = default!;
    public List<BLL.DTO.V1.ContentDto> Body { get; set; } = default!;
    

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