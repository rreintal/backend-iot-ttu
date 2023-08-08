using AutoMapper;
using Base.DAL;
using BLL.DTO.V1;

namespace App.BLL.Mappers;

public class ProjectsMapper : BaseMapper<Project, Domain.Project>
{
    public ProjectsMapper(IMapper mapper) : base(mapper)
    {
    }
}