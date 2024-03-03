using Base.Domain;
using BLL.DTO.Contracts;

namespace BLL.DTO.V1;

public class PartnerImage : DomainEntityId, IContainsImage
{
    public string? Link { get; set; }
    public string Image { get; set; } = default!;
}