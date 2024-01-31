using Base.Domain;

namespace BLL.DTO.Identity;

public class AppUser : DomainEntityId
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool EmailConfirmed { get; set; } = default!;
    public bool LockoutEnabled { get; set; } = default!;
    public List<BLL.DTO.Identity.AppRole> UserRoles { get; set; } = default!;
}