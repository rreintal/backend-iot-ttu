using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using DAL.DTO.Identity;

namespace App.BLL.Services;

public class UsersService : BaseEntityService<global::BLL.DTO.Identity.AppUser, AppUser, IUsersRepository>, IUsersService
{
    private IAppUOW Uow { get; }
    public UsersService(IAppUOW uow, IMapper<global::BLL.DTO.Identity.AppUser, global::DAL.DTO.Identity.AppUser> mapper) : base(uow.UsersRepository, mapper)
    {
        Uow = uow;
    }
}