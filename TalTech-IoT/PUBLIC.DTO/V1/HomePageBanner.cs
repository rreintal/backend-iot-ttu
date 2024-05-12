using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class HomePageBanner : DomainEntityId
{
    [IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    
    public string Image { get; set; } = default!;
    public int? SequenceNumber { get; set; }
}