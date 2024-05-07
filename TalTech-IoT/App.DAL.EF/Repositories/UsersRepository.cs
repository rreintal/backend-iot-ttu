using App.DAL.Contracts;
using App.Domain.Identity;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppUser = DAL.DTO.Identity.AppUser;

namespace App.DAL.EF.Repositories;

public class UsersRepository : EFBaseRepository<AppUser, AppDbContext>, IUsersRepository
{
    public UsersRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {}

    
    public override async Task<IEnumerable<AppUser>> AllAsync()
    {
        var res = (await DbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.AppRole)
            .Select(user => new Domain.Identity.AppUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                UserRoles = user.UserRoles.Select(ur => new AppUserRole
                {
                    AppRole = new AppRole
                    {
                        Id = ur.RoleId,
                        Name = ur.AppRole!.Name,
                    }
                }).ToList()
            })
            .ToListAsync());
        var result = res.Select(e => _mapper.Map<global::DAL.DTO.Identity.AppUser>(e));
            
        return result;
    }

    public async Task<IEnumerable<AppUser>> AllAsyncFiltered()
    {
        var res = (await DbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.AppRole)
            .Select(user => new Domain.Identity.AppUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                UserRoles = user.UserRoles.Select(ur => new AppUserRole
                {
                    AppRole = new AppRole
                    {
                        Id = ur.RoleId,
                        Name = ur.AppRole!.Name,
                    }
                }).ToList()
            })
            .ToListAsync());
        var result = res.Select(e => _mapper.Map<global::DAL.DTO.Identity.AppUser>(e));
        return result;
    }
}