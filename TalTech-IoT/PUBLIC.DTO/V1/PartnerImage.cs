using Base.Domain;

namespace Public.DTO.V1;

public class PartnerImage : DomainEntityId
{
    public string Image { get; set; } = default!;
}