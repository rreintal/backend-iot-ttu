using App.Domain;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1.FilterObjects;
using TopicArea = App.Domain.TopicArea;

namespace App.DAL.Contracts;

public interface ITopicAreaRepository : IBaseTranslateableRepository<App.Domain.TopicArea>, ITopicAreaRepositoryCustom<App.Domain.TopicArea>
{
    // methods only for repo
    public Task<IEnumerable<HasTopicArea>> GetHasTopicArea(TopicAreaCountFilter filter, string languageCulture);
}

public interface ITopicAreaRepositoryCustom<TEntity>
{
    // methods for repo and service
    //public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreasWithCount(TopicAreaCountFilter filter);
    
}
