using App.BLL.Contracts;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1.FeedPage;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers.FeedPage;

/// <summary>
/// Controller for FeedPageCategory
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class FeedPageCategoryController : ControllerBase
{
    private IAppBLL _bll;

    /// <inheritdoc />
    public FeedPageCategoryController(IAppBLL bll)
    {
        _bll = bll;
    }


    /// <summary>
    /// Get Feed Page Category with all of its Posts (all langs)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<FeedPageCategory>> Get(Guid id)
    {
        var bllEntity = await _bll.FeedPageCategoryService.FindAsync(id);
        var result = FeedPageCategoryMapper.Map(bllEntity);
        return result;
    }

    /// <summary>
    /// Delete FeedPageCategory (NOT IMPLEMENTO)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create new FeedPageCategory 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<FeedPageCategory>> Post(FeedPageCategory entity)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPageCategoryMapper.Map(entity, contentTypes);
        var bllResult = _bll.FeedPageCategoryService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        var result = FeedPageCategoryMapper.Map(bllResult);
        return Ok(result);
    }
}