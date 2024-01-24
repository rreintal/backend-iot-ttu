using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain.Identity;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class UsersService : BaseEntityService<AppUser, AppUser, IUsersRepository>, IUsersService
{
    private IAppUOW Uow { get; }
    public UsersService(IAppUOW uow, IMapper<AppUser, AppUser> mapper) : base(uow.UsersRepository, mapper)
    {
        Uow = uow;
    }
}