using Base.Domain;

namespace BLL.DTO.V1;

public class PartnerImage : DomainEntityId
{
    public string? Link { get; set; }
    public string Image { get; set; } = default!;
}