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
    private IAppBLL _bll;

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
    [HttpGet]
    public async Task<FeedPagePost> GetAllLanguages(Guid id)
    {
        var bllEntity = await _bll.FeedPagePostService.FindAsync(id);
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
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPagePostMapper.MapForUpdate(entity, contentTypes, entity.Id);
        var bllResult = await _bll.FeedPagePostService.UpdateAsync(bllEntity);
        var result = FeedPagePostMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return Ok(result);
    }

    [HttpDelete]
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
}