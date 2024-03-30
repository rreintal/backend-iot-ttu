using App.DAL.Contracts;
using Base.DAL.EF.Contracts;

namespace App.BLL.Contracts;

public interface IAppBLL : IBaseUOW
{
    INewsService NewsService { get; }
    ITopicAreaService TopicAreaService { get; }
    
    IMailService MailService { get; }
    
    IProjectService ProjectService { get; }
    
    IThumbnailService ThumbnailService { get; }
    
    IUsersService UsersService { get; }
    
    IPageContentService PageContentService { get; }
    
    IPartnerImageService PartnerImageService { get; }
    
    IHomePageBannerService HomePageBannerService { get; }
    
    IContactPersonService ContactPersonService { get; }
    
    IFeedPageCategoryService FeedPageCategoryService { get; }
    
    IFeedPagePostService FeedPagePostService { get; }
    
    IFeedPageService FeedPageService { get; }
    
    IOpenSourceSolutionService OpenSourceSolutionService { get; }
    IEmailValidationService EmailValidationService { get; }
    
}