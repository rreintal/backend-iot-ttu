using App.Domain;
using App.Domain.Translations;
using AutoMapper;

namespace BLL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DAL.DTO.V1.Content, BLL.DTO.V1.Content>().ReverseMap();
        CreateMap<DAL.DTO.V1.News, BLL.DTO.V1.News>().ReverseMap();
        CreateMap<DAL.DTO.V1.LanguageString, BLL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<DAL.DTO.V1.LanguageStringTranslation, BLL.DTO.V1.LanguageStringTranslation>().ReverseMap();
        CreateMap<DAL.DTO.V1.ContentType, BLL.DTO.V1.ContentType>().ReverseMap();
        
        CreateMap<Content, BLL.DTO.V1.Content>().ReverseMap();
        CreateMap<News, BLL.DTO.V1.News>().ReverseMap();
        CreateMap<LanguageString, BLL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<LanguageStringTranslation, BLL.DTO.V1.LanguageStringTranslation>().ReverseMap();
        CreateMap<ContentType, BLL.DTO.V1.ContentType>().ReverseMap();

        
    }
}