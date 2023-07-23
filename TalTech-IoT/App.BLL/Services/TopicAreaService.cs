using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Services;

public class TopicAreaService : BaseEntityService<TopicArea, Domain.TopicArea, ITopicAreaRepository>, ITopicAreaService
{
    private IAppUOW Uow { get; set; }
    
    public TopicAreaService(IAppUOW uow, IMapper<TopicArea, TopicArea> mapper) : base(uow.TopicAreaRepository, mapper)
    {
        Uow = uow;
    }
}