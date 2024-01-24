using App.Domain.Identity;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UsersMapper : BaseMapper<AppUser, AppUser>
{
    
    public UsersMapper(IMapper mapper) : base(mapper)
    {
    }
}