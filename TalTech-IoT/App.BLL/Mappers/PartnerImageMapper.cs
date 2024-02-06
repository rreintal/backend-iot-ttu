using App.Domain;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PartnerImageMapper : BaseMapper<global::BLL.DTO.V1.PartnerImage, PartnerImage>
{
    public PartnerImageMapper(IMapper mapper) : base(mapper)
    {
    }
}