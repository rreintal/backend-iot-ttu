using App.DAL.Contracts;
using App.Domain.Identity;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UsersRepository : EFBaseRepository<App.Domain.Identity.AppUser, AppDbContext>, IUsersRepository
{
    public UsersRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {}

    public override async Task<IEnumerable<AppUser>> AllAsync()
    {
        
        var res = await DbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.AppRole)
            .Select(user => new AppUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                AppRefreshTokens = user.AppRefreshTokens,
                UserRoles = user.UserRoles.Select(ur => new AppUserRole
                {
                    AppRole = new AppRole
                    {
                        Name = ur.AppRole!.Name,
                    }
                }).ToList()
            })
            .ToListAsync();

        return res;
        
    }
    
}