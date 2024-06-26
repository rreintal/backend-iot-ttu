using AutoMapper;
using BLL.DTO.V1;
using Content = App.Domain.Content;
using ContentType = App.Domain.ContentType;
using LanguageString = App.Domain.Translations.LanguageString;
using LanguageStringTranslation = App.Domain.Translations.LanguageStringTranslation;
using News = App.Domain.News;

namespace BLL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        
        CreateMap<Content, BLL.DTO.V1.Content>().ReverseMap();
        CreateMap<News, BLL.DTO.V1.News>().ReverseMap();
        CreateMap<LanguageString, BLL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<LanguageStringTranslation, BLL.DTO.V1.LanguageStringTranslation>().ReverseMap();
        CreateMap<ContentType, BLL.DTO.V1.ContentType>().ReverseMap();

        CreateMap<App.Domain.Translations.LanguageString, BLL.DTO.V1.LanguageString>().ReverseMap();

        CreateMap<App.Domain.News, BLL.DTO.V1.News>()
            .ForMember(dest => dest.TopicAreas,
                src => src.MapFrom(x => x.HasTopicAreas));
        
        CreateMap<App.Domain.Content, BLL.DTO.V1.Content>().ReverseMap();
        CreateMap<App.Domain.TopicArea, BLL.DTO.V1.TopicArea>().ReverseMap();
        CreateMap<BLL.DTO.V1.TopicArea, DAL.DTO.V1.TopicArea>().ReverseMap();

        CreateMap<App.Domain.Project, BLL.DTO.V1.Project>().ReverseMap();
        
        CreateMap<App.Domain.HasTopicArea, BLL.DTO.V1.TopicArea>()
            .ForMember(dest => dest.Id,
                src => src.MapFrom(x => x.TopicAreaId))
            .ForMember(d => d.LanguageString,
                s => s.MapFrom(x => x.TopicArea!.LanguageString))
            .ForMember(d => d.LanguageStringId,
                s => s.MapFrom(x => x.TopicArea!.LanguageStringId));

        CreateMap<BLL.DTO.V1.TopicArea, DAL.DTO.V1.TopicArea>();
        CreateMap<BLL.DTO.V1.UpdateNews, DAL.DTO.V1.UpdateNews>()
            .ReverseMap();
        CreateMap<BLL.DTO.V1.ContentDto, DAL.DTO.V1.ContentDto>()
            .ReverseMap();
        CreateMap<BLL.DTO.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppRole, DAL.DTO.Identity.AppRole>().ReverseMap();
        CreateMap<App.Domain.PageContent, BLL.DTO.V1.PageContent>().ReverseMap();
        
        
        CreateMap<BLL.DTO.V1.ContentType, DAL.DTO.V1.ContentType>().ReverseMap();
        CreateMap<BLL.DTO.V1.Content, DAL.DTO.V1.Content>().ReverseMap();
        CreateMap<BLL.DTO.V1.PageContent, DAL.DTO.V1.PageContent>().ReverseMap();
        CreateMap<BLL.DTO.V1.Project, DAL.DTO.V1.Project>().ReverseMap();
        CreateMap<BLL.DTO.V1.UpdateProject, DAL.DTO.V1.UpdateProject>().ReverseMap();
        
        CreateMap<BLL.DTO.V1.News, DAL.DTO.V1.News>().ReverseMap();
        CreateMap<BLL.DTO.V1.LanguageString, DAL.DTO.V1.LanguageString>().ReverseMap();
        CreateMap<BLL.DTO.V1.Content, DAL.DTO.V1.Content>().ReverseMap();
        CreateMap<BLL.DTO.V1.LanguageStringTranslation, DAL.DTO.V1.LanguageStringTranslation>().ReverseMap();

        CreateMap<App.Domain.PartnerImage, BLL.DTO.V1.PartnerImage>().ReverseMap();

        CreateMap<App.Domain.HomePageBanner, BLL.DTO.V1.HomePageBanner>().ReverseMap();

        CreateMap<UpdateHomePageBanner, DAL.DTO.V1.UpdateHomePageBanner>().ReverseMap();

        CreateMap<App.Domain.HomePageBanner, BLL.DTO.V1.UpdateHomePageBanner>().ReverseMap();
        CreateMap<App.Domain.ContactPerson, BLL.DTO.V1.ContactPerson>().ReverseMap();

        CreateMap<App.Domain.FeedPage, BLL.DTO.V1.FeedPage>().ReverseMap();
        CreateMap<App.Domain.FeedPageCategory, BLL.DTO.V1.FeedPageCategory>().ReverseMap();
        CreateMap<App.Domain.FeedPagePost, BLL.DTO.V1.FeedPagePost>().ReverseMap();
        CreateMap<App.Domain.FeedPageCategory, BLL.DTO.V1.FeedPageCategoryWithoutPosts>().ReverseMap();

        CreateMap<App.Domain.OpenSourceSolution, BLL.DTO.V1.OpenSourceSolution>().ReverseMap();
        CreateMap<DAL.DTO.V1.ImageResource, BLL.DTO.V1.ImageResource>().ReverseMap();
        CreateMap<App.Domain.ImageResource, BLL.DTO.V1.ImageResource>().ReverseMap();
        CreateMap<DAL.DTO.V1.AccessDetails, BLL.DTO.V1.AccessDetails>().ReverseMap();
        CreateMap<App.Domain.AccessDetails, BLL.DTO.V1.AccessDetails>().ReverseMap();
    }
}