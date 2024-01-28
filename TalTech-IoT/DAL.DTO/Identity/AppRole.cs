using Base.Domain;

namespace DAL.DTO.Identity;

public class AppRole : DomainEntityId
{
    public string Name { get; set; } = default!;
}