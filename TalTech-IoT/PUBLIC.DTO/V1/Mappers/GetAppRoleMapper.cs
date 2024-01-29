using Public.DTO.Identity;

namespace Public.DTO.V1.Mappers;

public static class GetAppRoleMapper
{
    public static Public.DTO.Identity.AppRole Map(App.Domain.Identity.AppRole role)
    {
        return new AppRole()
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}