using Base.BLL.Contracts;
using BLL.DTO.Identity;

namespace App.BLL.Contracts;

public interface IUsersService : IEntityService<AppUser>
{
    public Task<IEnumerable<AppUser>> AllAsyncFiltered(bool isDeleted);
}