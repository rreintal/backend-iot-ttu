using App.DAL.Contracts;
using Base.BLL.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1.FilterObjects;

namespace App.BLL.Contracts;

public interface ITopicAreaService : ITranslateableEntityService<TopicArea>, ITopicAreaRepositoryCustom<App.Domain.TopicArea>
{
    public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreaWithCount(TopicAreaCountFilter filter, string languageCulture);
    
}
