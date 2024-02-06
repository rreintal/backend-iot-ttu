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
    
}