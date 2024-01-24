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
}