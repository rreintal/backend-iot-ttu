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
    
    public FeedPageCategoryController(IAppBLL bll)
    {
        _bll = bll;
    }


    [HttpGet]
    public async Task<ActionResult<FeedPageCategory>> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create new FeedPageCategory 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Post(FeedPageCategory entity)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPageCategoryMapper.Map(entity, contentTypes);
        _bll.FeedPageCategoryService.Add(bllEntity);

        await _bll.SaveChangesAsync();
        return Ok();
    }
}