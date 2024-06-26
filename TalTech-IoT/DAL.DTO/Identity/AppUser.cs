using Base.Domain;

namespace DAL.DTO.Identity;

public class AppUser : DomainEntityId
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool EmailConfirmed { get; set; } = default!;
    public bool LockoutEnabled { get; set; } = default!;
    public bool Deleted { get; set; }
    public List<AppRole> UserRoles { get; set; } = default!;
}