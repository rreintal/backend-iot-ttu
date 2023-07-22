using Base.DAL.EF.Contracts;
using Public.DTO.V1;
using TopicArea = App.Domain.TopicArea;

namespace App.BLL.Contracts;

public interface ITopicAreaService : IBaseRepository<TopicArea>
{
    public Public.DTO.V1.TopicArea Add(CreateTopicAreaDto data);

}