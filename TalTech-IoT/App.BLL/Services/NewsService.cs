using App.BLL.Contracts;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using Public.DTO.V1.Mappers;

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

    public News Create(Public.DTO.V1.CreateNewsDto data)
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

        var mappedResult = CreateNewsMapper.Map(data, types);
        var result = Add(mappedResult);
        return result;
    }

    public async Task<IEnumerable<News>> GetNews()
    {
        return (await Uow.NewsRepository.AllAsync()).Select(e => _mapper.Map<News>(e));
    }
}