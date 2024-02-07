using Base.Domain;

namespace Public.DTO.V1;

public class UpdateHomePageBanner : DomainEntityId
{
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;
    
    public string? Image { get; set; }
}