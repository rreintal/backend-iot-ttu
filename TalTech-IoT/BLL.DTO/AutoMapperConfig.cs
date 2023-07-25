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

        CreateMap<App.Domain.Translations.LanguageString, BLL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<App.Domain.Content, BLL.DTO.V1.Content>().ReverseMap();
        CreateMap<App.Domain.LanguageStringTranslation, BLL.DTO.V1.LanguageStringTranslation>().ReverseMap();
        CreateMap<App.Domain.HasTopicArea, BLL.DTO.V1.TopicArea>()
            .ForMember(dest => dest.Id,
                src => src.MapFrom(x => x.TopicAreaId))
            .ForMember(d => d.ParentTopicArea,
                s => s.MapFrom(x => x.TopicArea!.ParentTopicArea))
            .ForMember(d => d.LanguageString,
                s => s.MapFrom(x => x.TopicArea!.LanguageString))
            .ForMember(d => d.LanguageStringId,
                s => s.MapFrom(x => x.TopicArea!.LanguageStringId));

    }
}