using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.DAL;
using BLL.DTO.V1;


namespace App.BLL;

public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    protected readonly IAppUOW Uow;
    private readonly AutoMapper.IMapper _mapper;
    private readonly IImageStorageService _imageStorageService;

    public AppBLL(IAppUOW uow, IMapper mapper, IImageStorageService imageStorageService) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
        _imageStorageService = imageStorageService;
    }

    private INewsService? _newsService;

    public INewsService NewsService =>
        _newsService ??= new NewsService(Uow, new NewsMapper(_mapper), _mapper, ThumbnailService, _imageStorageService);

    
    private ITopicAreaService? _TopicAreaService;

    public ITopicAreaService TopicAreaService =>
        _TopicAreaService ??= new TopicAreaService(Uow, new TopicAreaMapper(_mapper));

    private IMailService? _mailService;

    public IMailService MailService 
        => _mailService ??= new MailService();

    private IProjectService? _projectService;

    public IProjectService ProjectService =>
        _projectService ??= new ProjectService(Uow, new ProjectsMapper(_mapper), _mapper, ThumbnailService);

    public IThumbnailService ThumbnailService => _thumbnailService ??= new ThumbnailService();

    private IThumbnailService? _thumbnailService;

    public IUsersService UsersService =>
        _usersService ??= new UsersService(Uow, new UsersMapper(_mapper));

    private IUsersService? _usersService;

    private IPageContentService? _pageContentService;
    public IPageContentService PageContentService =>
        _pageContentService ??= new PageContentService(Uow, new PageContentMapper(_mapper));

    public IPartnerImageService PartnerImageService =>
        _partnerImageService ??=
            new PartnerImageService(Uow, new BaseMapper<PartnerImage, Domain.PartnerImage>(_mapper));

    private IPartnerImageService? _partnerImageService;

    public IHomePageBannerService HomePageBannerService =>
        _homePageBannerService ??=
            new HomePageBannerService(Uow, new BaseMapper<HomePageBanner, Domain.HomePageBanner>(_mapper), _mapper);
    
    private IHomePageBannerService? _homePageBannerService;

    public IContactPersonService ContactPersonService => _contactPersonService ??=
        new ContactPersonService(Uow, new BaseMapper<ContactPerson, Domain.ContactPerson>(_mapper));

    private IContactPersonService? _contactPersonService;
    
    public IFeedPageCategoryService FeedPageCategoryService => _feedPageCategoryService ??=
        new FeedPageCategoryService(Uow, new BaseMapper<FeedPageCategory, Domain.FeedPageCategory>(_mapper), _mapper);

    private IFeedPageCategoryService? _feedPageCategoryService;

    public IFeedPagePostService FeedPagePostService => _feedPagePostService ??=
        new FeedPagePostService(Uow, new BaseMapper<FeedPagePost, Domain.FeedPagePost>(_mapper));

    private IFeedPagePostService? _feedPagePostService;

    public IFeedPageService FeedPageService => _feedPageService ??=
        new FeedPageService(Uow, new BaseMapper<FeedPage, Domain.FeedPage>(_mapper));

    private IFeedPageService? _feedPageService;

}