using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1.FeedPage;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers.FeedPage;

/// <summary>
/// Controller for FeedPagePost
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class FeedPagePostController : ControllerBase
{
    private readonly IAppBLL _bll;

    /// <inheritdoc />
    public FeedPagePostController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Get feedPagePost with both languages
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedPagePost>> GetAllLanguages(Guid id)
    {
        var bllEntity = await _bll.FeedPagePostService.FindAsync(id);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var result = FeedPagePostMapper.Map(bllEntity);
        return result;
    }

    /// <summary>
    /// Add new Feed Page Post
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<FeedPagePost> Post(FeedPagePost entity)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPagePostMapper.Map(entity, contentTypes);
        var bllResult = _bll.FeedPagePostService.Add(bllEntity);
        var result = FeedPagePostMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return result;
    }

    /// <summary>
    /// Update FeedPagePost content
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<FeedPagePost>> Update(FeedPagePost entity)
    {
        var isEntityFound = await _bll.FeedPagePostService.FindAsync(entity.Id) != null;
        if (!isEntityFound)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPagePostMapper.MapForUpdate(entity, contentTypes, entity.Id);
        var bllResult = await _bll.FeedPagePostService.UpdateAsync(bllEntity);
        var result = FeedPagePostMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Delete FeedPagePost by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _bll.FeedPagePostService.RemoveAsync(id);
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
    
    /// <summary>
    /// Get Feed Page Post by id translated
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    /*
    [HttpGet("{languageCulture}/{id}")]
    public async Task<ActionResult<Public.DTO.V1.FeedPage.FeedPagePost>> Get(Guid id, string languageCulture)
    {
        var bllEntity = await _bll.FeedPagePostService.FindAsync(id, languageCulture);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = FeedPagePostMapper.Map(bllEntity, languageCulture); 
        return Ok(result);
    }
    */
}