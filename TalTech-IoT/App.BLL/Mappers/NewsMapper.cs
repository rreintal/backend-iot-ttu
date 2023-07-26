using AutoMapper;
using Base.Contracts;
using Base.DAL;
using BLL.DTO.V1;

namespace App.BLL.Mappers;

public class NewsMapper : BaseMapper<News, App.Domain.News>
{
    public NewsMapper(IMapper mapper) : base(mapper)
    {
    }
    
}