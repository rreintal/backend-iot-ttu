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

        CreateMap<App.Domain.ContentType, DAL.DTO.V1.ContentType>().ReverseMap();
        CreateMap<App.Domain.Content, DAL.DTO.V1.Content>().ReverseMap();
        CreateMap<App.Domain.News, DAL.DTO.V1.News>().ReverseMap();
        CreateMap<App.Domain.PageContent, DAL.DTO.V1.PageContent>().ReverseMap();
        CreateMap<App.Domain.Project, DAL.DTO.V1.Project>().ReverseMap();

    }
}
