using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : EFBaseUOW<AppDbContext>, IAppUOW
{
    private IMapper _mapper { get; set; }
    public AppUOW(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
    {
        _mapper = mapper;
    }

    private INewsRepository? _newsRepository;
    public INewsRepository NewsRepository =>
        _newsRepository ??= new NewsRepository(UowDbContext, _mapper);

    private IContentTypesRepository? _contentTypesRepository;

    public IContentTypesRepository ContentTypesRepository =>
        _contentTypesRepository ??= new ContentTypesRepository(UowDbContext, _mapper);

    private ITopicAreaRepository? _topicAreaRepository;

    public ITopicAreaRepository TopicAreaRepository =>
        _topicAreaRepository ??= new TopicAreaRepository(UowDbContext, _mapper);

    public IProjectsRepository ProjectsRepository =>
        _projectsRepository ??= new ProjectsRepository(UowDbContext, _mapper);

    private IProjectsRepository? _projectsRepository;

    public IUsersRepository UsersRepository =>
        _usersRepository ??= new UsersRepository(UowDbContext, _mapper);

    private IUsersRepository? _usersRepository;

    private IPageContentRepository? _pageContentRepository;

    public IPageContentRepository PageContentRepository =>
        _pageContentRepository ??= new PageContentRepository(UowDbContext, _mapper);

    public IPartnerImageRepository PartnerImageRepository => 
        _partnerImageRepository ??= new PartnerImageRepository(UowDbContext, _mapper);

    private IPartnerImageRepository? _partnerImageRepository;

    public IHomePageBannerRepository HomePageBannerRepository =>
        _homePageBannerRepository ??= new HomePageBannerRepository(UowDbContext, _mapper);

    private IHomePageBannerRepository? _homePageBannerRepository;

    public IContactPersonRepository ContactPersonRepository => _contactPersonRepository ??=
        new ContactPersonRepository(UowDbContext, _mapper);

    private IContactPersonRepository? _contactPersonRepository;

    public IFeedPageCategoryRepository FeedPageCategoryRepository =>
        _feedPageCategoryRepository ??= new FeedPageCategoryRepository(UowDbContext, _mapper);
    
    private IFeedPageCategoryRepository? _feedPageCategoryRepository;
    public IFeedPageRepository FeedPageRepository =>
        _feedPageRepository ??= new FeedPageRepository(UowDbContext, _mapper);
    
    private IFeedPageRepository? _feedPageRepository;
    public IFeedPagePostRepository FeedPagePostRepository =>
        _feedPagePostRepository ??= new FeedPagePostRepository(UowDbContext, _mapper);

    private IFeedPagePostRepository? _feedPagePostRepository;

    public IOpenSourceSolutionRepository OpenSourceSolutionRepository =>
        _openSourceSolutionRepository ??= new OpenSourceSolutionRepository(UowDbContext, _mapper);

    private IOpenSourceSolutionRepository? _openSourceSolutionRepository;

}