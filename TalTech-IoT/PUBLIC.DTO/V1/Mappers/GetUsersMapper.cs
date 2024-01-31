using BLL.DTO.Identity;
using Public.DTO.Identity;

namespace Public.DTO.V1.Mappers;

public static class GetUsersMapper
{
    public static Public.DTO.Identity.AppUser Map(BLL.DTO.Identity.AppUser bll)
    {
        return new Public.DTO.Identity.AppUser()
        {
            Id = bll.Id,
            Email = bll.Email,
            Firstname = bll.Firstname,
            Lastname = bll.Lastname,
            UserName = bll.UserName,
            EmailConfirmed = bll.EmailConfirmed,
            LockoutEnabled = bll.LockoutEnabled,
            Roles = bll.UserRoles.Select(role => new Public.DTO.Identity.AppRole()
            {
                Id = role.Id,
                Name = role.Name
            }).ToList()
        };
    }
}