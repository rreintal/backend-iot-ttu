using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.DAL.Contracts;

public interface ITopicAreaRepository : IBaseTranslateableRepository<App.Domain.TopicArea>, ITopicAreaRepositoryCustom<App.Domain.TopicArea>
{
    // methods only for repo
    
}

public interface ITopicAreaRepositoryCustom<TEntity>
{
    // methods for repo and service
    public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(string languageCulture);
}
