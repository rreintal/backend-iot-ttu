using System.Net;
using App.BLL.Contracts;
using App.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;

//[Route("api/{languageCulture}/[controller]/[action]")]
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
    [HttpPost("api/[controller]/")]
    public async Task<IActionResult> Create([FromBody] PostNewsDto payload)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Console.WriteLine("Creating News!");
        
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
    [HttpGet("api/{languageCulture}/[controller]/")]
    public async Task<IEnumerable<Public.DTO.V1.News>> Get(string languageCulture, int? page, int? size)
    {
        // TODO - filter news by author/topic
        _bll.NewsService.SetLanguageStrategy(languageCulture);
        var news = (await _bll.NewsService.AllAsyncFiltered(page, size)).ToList();
        return news.Select(x => ReturnNewsMapper.Map(x, true));
    }

    /// <summary>
    /// Get specific News
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("api/{languageCulture}/[controller]/{id}")]
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
    [HttpPut("api/{languageCulture}/[controller]/")]
    public async Task<ActionResult> Update([FromBody] Public.DTO.V1.UpdateNews data)
    {
        // TODO - when updating, should we add the language culture which one we want to update?
        
        // TODO - updating is with both languages!!!
        var bllEntity = UpdateNewsMapper.Map(data);
        var result = await _bll.NewsService.UpdateNews(bllEntity);

        await _bll.SaveChangesAsync();

        return Ok(new RestApiResponse()
        {
            Message = $"Updated news. id={result.Id}",
            StatusCode = 200
        });
    }

    /// <summary>
    /// Delete news by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("api/[controller]/{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.NewsService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = $"News with id {id.ToString()} not found.",
                StatusCode = (int)HttpStatusCode.NotFound
            });
        }
        var result = _bll.NewsService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok(new
        {
            NewsId = result.Id
        });
    }
}