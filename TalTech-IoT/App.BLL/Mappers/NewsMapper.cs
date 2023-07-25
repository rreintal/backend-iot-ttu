using AutoMapper;
using Base.DAL;
using News = DAL.DTO.V1.News;

namespace App.BLL.Mappers;

public class NewsMapper : BaseMapper<global::BLL.DTO.V1.News, Domain.News>
{
    public NewsMapper(IMapper mapper) : base(mapper)
    {
    }
    
}