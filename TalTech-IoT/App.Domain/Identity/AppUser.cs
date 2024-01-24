using Contracts;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    
    //public ICollection<AppRole> Roles { get; set; } = default!;
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
    public ICollection<AppRole> Roles { get; set; } = default!;
}