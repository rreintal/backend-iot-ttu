using App.Domain.Identity;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IUsersRepository : IBaseRepository<AppUser>, IUsersRepositoryCustom<App.Domain.Identity.AppUser>
{
    
}

public interface IUsersRepositoryCustom<TEntity>
{
    // methods for repo and service
    //public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(TopicAreaCountFilter filter);
    
}