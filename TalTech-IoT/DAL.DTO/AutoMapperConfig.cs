using AutoMapper;

namespace DAL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.Domain.Content, DAL.DTO.V1.Content>().ReverseMap();
        CreateMap<App.Domain.News, DAL.DTO.V1.News>().ReverseMap();
        CreateMap<App.Domain.Translations.LanguageString, DAL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<App.Domain.LanguageStringTranslation, DAL.DTO.V1.LanguageStringTranslation>().ReverseMap();
        CreateMap<App.Domain.ContentType, DAL.DTO.V1.ContentType>().ReverseMap();
        
    }
}
