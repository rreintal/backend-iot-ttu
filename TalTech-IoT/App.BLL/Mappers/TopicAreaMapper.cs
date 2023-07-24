using AutoMapper;
using Base.DAL;
using BLL.DTO.V1;

namespace App.BLL.Mappers;

public class TopicAreaMapper : BaseMapper<TopicArea, App.Domain.TopicArea>
{
    public TopicAreaMapper(IMapper mapper) : base(mapper)
    {
    }
}