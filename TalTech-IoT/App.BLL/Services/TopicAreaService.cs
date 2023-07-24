using System.Runtime.Intrinsics.Arm;
using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Services;

public class TopicAreaService : BaseEntityService<TopicArea, Domain.TopicArea, ITopicAreaRepository>, ITopicAreaService
{
    // TODO - see peaks andma BLL dto tegelt!
    // DAL DTO on see MIS REPOSITORYST TULEB!
    // AGA IKKAGI BLL peaks saama panna ju?=!
    // vaata IEntityService<SEDA ENTITYT>
    private IAppUOW Uow { get; set; }
    
    public TopicAreaService(IAppUOW uow, IMapper<TopicArea, Domain.TopicArea> mapper) : base(uow.TopicAreaRepository, mapper)
    {
        Uow = uow;
    }
}