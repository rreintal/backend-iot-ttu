using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;


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
        _projectService ??= new ProjectService(Uow, new ProjectsMapper(_mapper), ThumbnailService);

    public IThumbnailService ThumbnailService => _thumbnailService ??= new ThumbnailService();

    private IThumbnailService? _thumbnailService;

    public IUsersService UsersService =>
        _usersService ??= new UsersService(Uow, new UsersMapper(_mapper));

    private IUsersService? _usersService;
}