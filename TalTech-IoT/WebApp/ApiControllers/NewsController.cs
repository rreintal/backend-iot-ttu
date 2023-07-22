using System.Collections.Immutable;
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
    protected IMapper _mapper;
    private readonly IAppBLL _bll;

    public NewsController(IMapper mapper, IAppBLL bll)
    {
        _mapper = mapper;
        _bll = bll;
    }

    [HttpPost]
    public async Task<string> Create([FromBody] CreateNewsDto payload)
    {
        var entity = _bll.NewsService.Create(payload);
        await _bll.SaveChangesAsync();
        return entity.Id.ToString();
    }

    [HttpGet]
    public async Task<IEnumerable<Public.DTO.V1.News>> GetNews(string languageCulture)
    {
        var news = (await _bll.NewsService.AllAsync()).ToList();
        return news.Select(x => ReturnNewsMapper.Map(x, languageCulture));
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
    
}