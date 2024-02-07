using Base.Domain;

namespace BLL.DTO.V1;

public class UpdateHomePageBanner : DomainEntityId
{
    public Guid Id { get; set; } = default!;
    public List<BLL.DTO.V1.ContentDto> Title { get; set; } = default!;
    public List<BLL.DTO.V1.ContentDto> Body { get; set; } = default!;
    public string? Image { get; set; }
}