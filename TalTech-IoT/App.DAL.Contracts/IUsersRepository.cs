using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IUsersRepository : IBaseRepository<global::DAL.DTO.Identity.AppUser> //IUsersRepositoryCustom<AppUser>
{
    
}

public interface IUsersRepositoryCustom<TEntity>
{
    // methods for repo and service
    //public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(TopicAreaCountFilter filter);
    
}