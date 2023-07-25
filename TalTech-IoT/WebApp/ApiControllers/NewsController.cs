using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

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

    /// <summary>
    /// Create new News
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get all News
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<Public.DTO.V1.News>> GetNews(string languageCulture)
    {
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var news = (await _bll.NewsService.AllAsync()).ToList();
        return news.Select(x => ReturnNewsMapper.Map(x));
    }

    /// <summary>
    /// Get specific News
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<Public.DTO.V1.News> GetById(Guid id, string languageCulture)
    {
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var bllEntity = await _bll.NewsService.FindAsync(id);
        var res = ReturnNewsMapper.Map(bllEntity);
        return res;
    }
    
    // TODO - filter news by author/topic
    
}