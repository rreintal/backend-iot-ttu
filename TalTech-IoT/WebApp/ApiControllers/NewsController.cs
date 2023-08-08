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
    public async Task<IActionResult> Create([FromBody] PostNewsDto payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
    public async Task<IEnumerable<Public.DTO.V1.News>> GetNews(string languageCulture, int? page, int? size)
    {
        // TODO - filter news by author/topic
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var news = (await _bll.NewsService.AllAsyncFiltered(page, size)).ToList();
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

    
    /// <summary>
    /// Update News
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Obsolete("Not implemented")]
    [HttpPut]
    public async Task<Public.DTO.V1.News> Update([FromBody] Public.DTO.V1.News data)
    {
        // TODO - when updating, should we add the language culture which one we want to update?
        // or should we just request both for updating?
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<Public.DTO.V1.DeleteNews> Delete([FromBody] Public.DTO.V1.DeleteNews data)
    {
        throw new NotImplementedException();
    }


}