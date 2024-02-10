using Base.Domain;

namespace Public.DTO.V1;

public class HomePageBanner : DomainEntityId
{
    // 1- 45 char
    
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;
    
    public string Image { get; set; } = default!;
    public int? SequenceNumber { get; set; }
}