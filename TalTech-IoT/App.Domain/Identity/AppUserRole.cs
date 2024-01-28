using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUserRole : IdentityUserRole<Guid>
{
    public virtual AppUser? AppUser { get; set; }
    public virtual AppRole? AppRole { get; set; }
    
}