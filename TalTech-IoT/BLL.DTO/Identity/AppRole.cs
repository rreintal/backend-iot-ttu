using Base.Domain;

namespace BLL.DTO.Identity;

public class AppRole : DomainEntityId
{
    public string Name { get; set; } = default!;

}