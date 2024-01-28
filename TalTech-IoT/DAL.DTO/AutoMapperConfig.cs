using AutoMapper;

namespace DAL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.Domain.Identity.AppUserRole, DAL.DTO.Identity.AppRole>()
            .ForMember(dest => dest.Id,
                src => src.MapFrom(x => x.AppRole!.Id))
            .ForMember(dest => dest.Name,
                src => src.MapFrom(x => x.AppRole!.Name))
            .ReverseMap();
        
        CreateMap<App.Domain.Identity.AppUser, DAL.DTO.Identity.AppUser>()
            .ReverseMap();

    }
}
