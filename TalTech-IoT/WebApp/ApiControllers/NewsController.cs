using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using App.DAL.EF;
using App.Domain.Helpers;
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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Public.DTO.V1.News>> Create([FromBody] PostNewsDto payload)
    {
        
        // TODO - TopicArea to optional
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = CreateNewsMapper.Map(payload, types);
        var entity = _bll.NewsService.Add(bllEntity);

        await _bll.SaveChangesAsync();
        
        var result = ReturnNewsMapper.Map(entity);
        return Ok(result) ;
    }

    /// <summary>
    /// Get all News
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("api/{languageCulture}/[controller]/")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<Public.DTO.V1.News>> Get(string languageCulture, NewsFilterSet filterSet)
    {
        // TODO - filter news by author/topic
        var news = (await _bll.NewsService.AllAsyncFiltered(filterSet, languageCulture)).ToList();
        return news.Select(x => ReturnNewsMapper.Map(x, true));
    }

    /// <summary>
    /// Get specific News
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("api/{languageCulture}/[controller]/{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Public.DTO.V1.News>> GetById(Guid id, string languageCulture)
    {
        var bllEntity = await _bll.NewsService.FindAsync(id, languageCulture);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiResponseError.NotFound,
                Status = HttpStatusCode.NotFound
            });
            
        }
        var res = ReturnNewsMapper.Map(bllEntity);
        return res;
    }

    /// <summary>
    /// Update News. Updateing with Topic Areas is not working YET!
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("api/[controller]/")]
    public async Task<ActionResult> Update([FromBody] Public.DTO.V1.UpdateNews data)
    {
        // TODO - when updating, should we add the language culture which one we want to update?
        
        // TODO - updating is with both languages!!!
        
        // TODO: ei tööta
        var bllEntity = UpdateNewsMapper.Map(data);
        var result = await _bll.NewsService.UpdateNews(bllEntity);

        await _bll.SaveChangesAsync();

        return Ok(new RestApiResponse()
        {
            Message = $"Updated news. id={result.Id}",
            Status = HttpStatusCode.OK
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
                Status = HttpStatusCode.NotFound
            });
        }
        var result = _bll.NewsService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok(new
        {
            NewsId = result.Id
        });
    }

    [HttpGet("api/[controller]/News/Count")]
    public async Task<int> CountAllNews()
    {
        return await _bll.NewsService.FindNewsTotalCount();
    }
    

    [HttpGet("api/[controller]/Preview/{id}")]
    public async Task<Public.DTO.V1.NewsAllLangs> GetNewsAllLanguages(Guid id)
    {
        var entity = await _bll.NewsService.FindByIdAllTranslationsAsync(id);
        return NewsAllLangMapper.Map(entity);
    }

}