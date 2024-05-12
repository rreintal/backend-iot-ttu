using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Public.DTO.V1;

public class PartnerImage : DomainEntityId
{
    [MinLength(2)]
    [MaxLength(1024)]
    public string? Link { get; set; }
    public string Image { get; set; } = default!;
}