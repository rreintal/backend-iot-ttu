using App.BLL.Contracts;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;


namespace App.BLL.Services;

public class NewsService : BaseEntityService<News, Domain.News, INewsRepository>, INewsService
{
    private IAppUOW Uow { get; set; }
    private IMapper _mapper { get; }
    
    // need Add, Remove jne on basic operationid
    // kui vaja tagastada DTO siis seda tehakse custom meetoditega!!
    public NewsService(IAppUOW uow, IMapper<News, Domain.News> mapper, IMapper autoMapper) : base(uow.NewsRepository, mapper)
    {
        Uow = uow;
        _mapper = autoMapper;
    }

    public News FindById(Guid id)
    {
        var item = Uow.NewsRepository.FindById(id).Result;
        return _mapper.Map<News>(item); 
    }

    public async Task<IEnumerable<News>> GetNews()
    {
        
        return (await Uow.NewsRepository.AllAsync()).Select(e => _mapper.Map<News>(e));
    }
    
    public async Task<List<ContentType>> GetContentTypes()
    {
        var titleContentType = Uow.ContentTypesRepository.FindByName("TITLE");
        var bodyContentType = Uow.ContentTypesRepository.FindByName("BODY");
        
        var body = new ContentType()
        {
            Id = bodyContentType.Id,
            Name = bodyContentType.Name
        };

        var title = new ContentType()
        {
            Id = titleContentType.Id,
            Name = titleContentType.Name
        };
        var types = new List<ContentType>()
        {
            body, title
        };
        return types;
    }
    
}