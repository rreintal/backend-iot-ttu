using System.Net.Mime;
using App.BLL.Contracts;
using App.DAL.Contracts;
using App.DAL.EF;
using App.Domain;
using App.Domain.Translations;
using AutoMapper;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;
using News = App.Domain.News;

namespace WebApp.ApiControllers;

//[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Route("api/{languageCulture}/[controller]/[action]")]
public class NewsController : ControllerBase
{
    protected AppDbContext _context;
    protected IMapper _mapper;
    private readonly IAppBLL _bll;
    

    public NewsController(AppDbContext context, IMapper mapper, IAppBLL bll)
    {
        _context = context;
        _mapper = mapper;
        _bll = bll;
    }

    [HttpPost]
    public async Task<string> Create([FromBody] NewsDTO payload)
    {
        // TODO mapper
        var newsId = Guid.NewGuid();
        var bodyContentType = _context.ContentTypes.Where(x => x.Name == "BODY").AsNoTracking().First();
        var titleContentType = _context.ContentTypes.Where(x => x.Name == "TITLE").AsNoTracking().First();
        
        // TEST - mapper
        var body = new BLL.DTO.V1.ContentType()
        {
            Id = bodyContentType.Id,
            Name = bodyContentType.Name
        };

        var title = new BLL.DTO.V1.ContentType()
        {
            Id = titleContentType.Id,
            Name = titleContentType.Name
        };
        var types = new List<BLL.DTO.V1.ContentType>()
        {
            body, title
        };
        // BLL DTO
        BLL.DTO.V1.News mappedResult = CreateNewsMapper.Map(payload, types);
        _bll.NewsService.Add(mappedResult);
        
        await _bll.SaveChangesAsync();
        return mappedResult.Id.ToString();
    }
    
    [HttpGet]
    public async Task<Public.DTO.V1.News> GetById(Guid id, string languageCulture)
    {
        var query = _bll.NewsService.FindById(id);
        
        // this should be public mapper!
        var title = query!.Content.First(x => x.ContentType!.Name == "TITLE")
            .LanguageString
            .LanguageStringTranslations.First(x => x.LanguageCulture == languageCulture).TranslationValue;
        
        var body = query.Content.First(x => x.ContentType!.Name == "TITLE")
            .LanguageString
            .LanguageStringTranslations.First(x => x.LanguageCulture == languageCulture).TranslationValue;
        
        var res = new Public.DTO.V1.News()
        {
            Id = query.Id,
            Title = title,
            Body = body,
            CreatedAt = query.CreatedAt
        };

        return res;
    }
    
    
    [HttpPost]
    public async Task<int> AddContentType([FromBody] CreateContentDTO data)
    {
        Console.WriteLine($"Typename: {data.TypeName}");
        
        var res = new App.Domain.ContentType()
        {
            Name = data.TypeName
        };
        await _context.ContentTypes.AddAsync(res);
        return await _context.SaveChangesAsync();
    }

    public class CreateContentDTO
    {
        public string TypeName { get; set; } = default!;
    }
}