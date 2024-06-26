﻿using AutoMapper;
using Microsoft.AspNetCore.RateLimiting;

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
        CreateMap<App.Domain.Project, DAL.DTO.V1.UpdateProject>().ReverseMap();

        CreateMap<App.Domain.Translations.LanguageString, DAL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<App.Domain.Translations.LanguageStringTranslation, DAL.DTO.V1.LanguageStringTranslation>().ReverseMap();


        CreateMap<App.Domain.News, DAL.DTO.V1.News>().ReverseMap();
        CreateMap<App.Domain.News, DAL.DTO.V1.News>()
            .ForMember(dest => dest.TopicAreas,
                src => src.MapFrom(x => x.HasTopicAreas));
        
        CreateMap<App.Domain.HasTopicArea, DAL.DTO.V1.TopicArea>()
            .ForMember(dest => dest.Id,
                src => src.MapFrom(x => x.TopicAreaId))
            .ForMember(d => d.LanguageString,
                s => s.MapFrom(x => x.TopicArea!.LanguageString))
            .ForMember(d => d.LanguageStringId,
                s => s.MapFrom(x => x.TopicArea!.LanguageStringId));

        CreateMap<App.Domain.HomePageBanner, DAL.DTO.V1.UpdateHomePageBanner>().ReverseMap();

        CreateMap<App.Domain.News, DAL.DTO.V1.UpdateNews>().ReverseMap();

        CreateMap<App.Domain.OpenSourceSolution, DAL.DTO.V1.OpenSourceSolution>().ReverseMap();
        CreateMap<App.Domain.ImageResource, DAL.DTO.V1.ImageResource>().ReverseMap();
        CreateMap<App.Domain.AccessDetails, DAL.DTO.V1.AccessDetails>().ReverseMap();
        
        CreateMap<App.Domain.News, DAL.DTO.V1.News>()
            .ForMember(dest => dest.TopicAreas,
                opt => opt.MapFrom(src => src.HasTopicAreas.Select(ha => ha.TopicAreaId)));
        
        CreateMap<App.Domain.HasTopicArea, DAL.DTO.V1.TopicArea>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TopicAreaId));
        
        CreateMap<App.Domain.News, DAL.DTO.V1.News>()
            .ForMember(dest => dest.TopicAreas,
                opt => opt.MapFrom(src => src.HasTopicAreas)); 
        
    }
}
