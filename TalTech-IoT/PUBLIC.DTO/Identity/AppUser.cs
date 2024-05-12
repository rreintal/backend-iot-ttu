namespace Public.DTO.Identity;

public class AppUser
{
    public Guid Id { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public bool EmailConfirmed { get; set; } 
    public bool LockoutEnabled { get; set; } 
    public bool Deleted { get; set; }
    public List<AppRole> Roles { get; set; } = default!;
}