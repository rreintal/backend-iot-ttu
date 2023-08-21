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

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
    }

    private INewsService? _newsService;

    public INewsService NewsService =>
        _newsService ??= new NewsService(Uow, new NewsMapper(_mapper), _mapper, ThumbnailService);

    
    private ITopicAreaService? _TopicAreaService;

    public ITopicAreaService TopicAreaService =>
        _TopicAreaService ??= new TopicAreaService(Uow, new TopicAreaMapper(_mapper));

    private IMailService? _mailService;

    public IMailService MailService 
        => _mailService ??= new MailService();

    private IProjectService? _projectService;

    public IProjectService ProjectService =>
        _projectService ??= new ProjectService(Uow, new ProjectsMapper(_mapper));

    public IThumbnailService ThumbnailService => _thumbnailService ??= new ThumbnailService();

    private IThumbnailService? _thumbnailService;
}