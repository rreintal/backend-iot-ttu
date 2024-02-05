using Base.DAL.EF.Contracts;
using BLL.DTO.Identity;
using AppUser = DAL.DTO.Identity.AppUser;

namespace App.DAL.Contracts;

public interface IUsersRepository : IBaseRepository<global::DAL.DTO.Identity.AppUser> //IUsersRepositoryCustom<AppUser>
{
    public Task<IEnumerable<AppUser>> AllAsyncFiltered(bool isDeleted);
}

public interface IUsersRepositoryCustom<TEntity>
{
    // methods for repo and service
    //public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(TopicAreaCountFilter filter);
    
}