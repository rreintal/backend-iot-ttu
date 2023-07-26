using App.DAL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1.FilterObjects;

namespace App.BLL.Contracts;

public interface ITopicAreaService : IBaseRepository<TopicArea>, ITopicAreaRepositoryCustom<App.Domain.TopicArea>
{
    public Task<IEnumerable<TopicAreaWithCount>> GetTopicAreaWithCount(TopicAreaCountFilter filter);
}
