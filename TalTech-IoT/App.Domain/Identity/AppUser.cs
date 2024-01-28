using Contracts;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;

    public virtual ICollection<AppUserRole>? UserRoles { get; set; }
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
    
}