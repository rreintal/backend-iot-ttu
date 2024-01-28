using AutoMapper;
using Base.DAL;
using AppUser = BLL.DTO.Identity.AppUser;

namespace App.BLL.Mappers;

public class UsersMapper : BaseMapper<AppUser, global::DAL.DTO.Identity.AppUser>
{
    
    public UsersMapper(IMapper mapper) : base(mapper)
    {
    }
}