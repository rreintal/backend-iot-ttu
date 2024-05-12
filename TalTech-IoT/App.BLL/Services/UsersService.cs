using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using DAL.DTO.Identity;

namespace App.BLL.Services;

public class UsersService : BaseEntityService<global::BLL.DTO.Identity.AppUser, AppUser, IUsersRepository>, IUsersService
{
    private IAppUOW Uow { get; }
    public UsersService(IAppUOW uow, IMapper<global::BLL.DTO.Identity.AppUser, AppUser> mapper) : base(uow.UsersRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<global::BLL.DTO.Identity.AppUser>> AllAsyncFiltered()
    {
        return (await Uow.UsersRepository.AllAsyncFiltered()).Select(entity => Mapper.Map(entity));
    }
}