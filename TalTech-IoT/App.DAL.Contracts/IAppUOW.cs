using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IAppUOW : IBaseUOW
{
    // this is app level contract, which specifies what rules the app level UOW has to follow
    // for example repositories
    
    INewsRepository NewsRepository { get; }
    
    IContentTypesRepository ContentTypesRepository { get; }

    ITopicAreaRepository TopicAreaRepository { get; }
    
    IProjectsRepository ProjectsRepository { get; }
    
    IUsersRepository UsersRepository { get; }
    
    IPageContentRepository PageContentRepository { get; }
    
    IPartnerImageRepository PartnerImageRepository { get; }
    
    IHomePageBannerRepository HomePageBannerRepository { get; }
    
    IContactPersonRepository ContactPersonRepository { get; }
    
    IFeedPageCategoryRepository FeedPageCategoryRepository { get; }
    
    IFeedPageRepository FeedPageRepository { get; }
    
    IFeedPagePostRepository FeedPagePostRepository { get; }
    
    IOpenSourceSolutionRepository OpenSourceSolutionRepository { get; }
    
    IAccessDetailsRepository AccessDetailsRepository { get; }
}