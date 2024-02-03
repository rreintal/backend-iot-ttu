using App.Domain;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PageContentMapper : BaseMapper<global::BLL.DTO.V1.PageContent, PageContent>
{
    public PageContentMapper(IMapper mapper) : base(mapper)
    {
    }
}