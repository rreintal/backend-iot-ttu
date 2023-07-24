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
    private readonly IAppBLL _bll;

    public NewsController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNewsDto payload)
    {
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = CreateNewsMapper.Map(payload, types);
        
        var entity = _bll.NewsService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok(new
        {
            NewsId = entity.Id.ToString()
        });
    }

    [HttpGet]
    public async Task<IEnumerable<Public.DTO.V1.News>> GetNews(string languageCulture)
    {
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var news = (await _bll.NewsService.AllAsync()).ToList();
        return news.Select(x => ReturnNewsMapper.Map(x));
    }
    
    [HttpGet]
    public async Task<Public.DTO.V1.News> GetById(Guid id, string languageCulture)
    {
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var bllEntity = _bll.NewsService.FindById(id);
        var res = ReturnNewsMapper.Map(bllEntity);
        return res;
    }
    
    // TODO - filter news by author/topic
    
}