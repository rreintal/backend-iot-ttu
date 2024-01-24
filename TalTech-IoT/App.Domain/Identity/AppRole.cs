using Contracts;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    public override string Name { get; set; } = default!;
}