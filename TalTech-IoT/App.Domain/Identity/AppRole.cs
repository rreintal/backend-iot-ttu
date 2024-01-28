using Contracts;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    public virtual ICollection<AppUserRole>? UserRoles { get; set; }
}