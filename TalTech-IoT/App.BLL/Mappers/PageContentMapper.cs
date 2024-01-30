using App.Domain;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PageContentMapper : BaseMapper<PageContent, PageContent>
{
    public PageContentMapper(IMapper mapper) : base(mapper)
    {
    }
}