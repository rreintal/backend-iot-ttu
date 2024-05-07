using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.ApiExceptions;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;
using Public.DTO.V1;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for News
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly IAppBLL _bll;
    
    /// <summary>
    /// Controller for News
    /// </summary>
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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(News), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RestApiResponse), StatusCodes.Status409Conflict)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<News>> Add([FromBody] PostNewsDto payload, [FromQuery] bool? test)
    {
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = CreateNewsMapper.Map(payload, types);
        try
        {
            var isTest = test ?? false;
            var bllResult = await _bll.NewsService.AddAsync(bllEntity, isTest);
            var result = ReturnNewsMapper.Map(bllResult);
            await _bll.SaveChangesAsync();
            return Ok(result);
        }
        catch (TopicAreasNotUnique ex)
        {
            return Conflict(new RestApiResponse()
            {
                Message = "TOPIC_AREAS_NOT_UNIQUE",
                Status = HttpStatusCode.Conflict
            });
        }
        
    }

    /// <summary>
    /// Get all News
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<News>> Get(string languageCulture, int? Size, int? page, Guid? TopicAreaId, bool? IncludeBody)
    {
        var filterSet = new NewsFilterSet()
        {
            IncludeBody = IncludeBody ?? false,
            Page = page,
            Size = Size,
            TopicAreaId = TopicAreaId
        };
        
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
    [HttpGet("{languageCulture}/{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<News>> GetById(Guid id, string languageCulture)
    {
        var bllEntity = await _bll.NewsService.FindAsync(id, languageCulture);
        await IncreaseViewCount(id);
        
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
            
        }
        
        var res = ReturnNewsMapper.Map(bllEntity);
        return Ok(res);
    }

    private async Task IncreaseViewCount(Guid id)
    {
        var isClientHeaderPresent = HttpContext.Request.Headers.ContainsKey("IOT-App");
        if (isClientHeaderPresent)
        {
            await _bll.NewsService.IncrementViewCount(id);
            await _bll.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Update News.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Update([FromBody] Public.DTO.V1.UpdateNews data, [FromQuery] bool? test)
    {
        try
        {
            var contentTypes = await _bll.NewsService.GetContentTypes();
            var bllEntity = UpdateNewsMapper.Map(data, contentTypes);
            var isTest = test ?? false;
            var result =  await _bll.NewsService.UpdateAsync(bllEntity, isTest);
            if (result == null)
            {
                return NotFound(new RestApiResponse()
                {
                    Message = RestApiErrorMessages.GeneralNotFound,
                    Status = HttpStatusCode.NotFound
                });
            }

            await _bll.SaveChangesAsync();
            return Ok();   
        }
        catch (TopicAreasNotUnique ex)
        {
            return Conflict(new RestApiResponse()
            {
                Message = "TOPIC_AREAS_NOT_UNIQUE",
                Status = HttpStatusCode.Conflict
            });
        }
    }

    /// <summary>
    /// ProcessDelete news by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete(Guid id, [FromQuery] bool? test)
    {
        var entity = await _bll.NewsService.FindByIdAllTranslationsAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var isTest = test ?? false;
        await _bll.NewsService.RemoveAsync(entity, isTest);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Returns the count of all news by TopicArea
    /// </summary>
    /// <returns></returns>
    [HttpGet("Count/{TopicAreaId}")]
    public async Task<int> CountAllNews(Guid? TopicAreaId)
    {
        return await _bll.NewsService.FindNewsTotalCount(TopicAreaId);
    }
    
    /// <summary>
    /// Returns the count of all news
    /// </summary>
    /// <returns></returns>
    [HttpGet("Count")]
    public async Task<int> CountAllNews()
    {
        return await _bll.NewsService.FindNewsTotalCount(null);
    }
    

    /// <summary>
    /// Returns news with all the language cultures
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Preview/{id}")]
    public async Task<ActionResult<Public.DTO.V1.NewsAllLangs>> GetNewsAllLanguages(Guid id)
    {
        var entity = await _bll.NewsService.FindByIdAllTranslationsAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });

        }
        return NewsAllLangMapper.Map(entity);
    }

}