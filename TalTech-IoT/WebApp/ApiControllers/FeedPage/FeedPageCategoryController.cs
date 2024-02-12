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
/// Controller for FeedPageCategory
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class FeedPageCategoryController : ControllerBase
{
    private readonly IAppBLL _bll;

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
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedPageCategory>> Get(Guid id)
    {
        var bllEntity = await _bll.FeedPageCategoryService.FindAsync(id);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var result = FeedPageCategoryMapper.Map(bllEntity);
        return result;
    }

    /// <summary>
    /// Delete FeedPageCategory (fails if Feed Page Category has Feed Page Posts) 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var categoryHasPosts = await _bll.FeedPageCategoryService.DoesCategoryHavePostsAsync(id);
        if (categoryHasPosts)
        {
            return Conflict(new RestApiResponse()
            {
                Message = RestApiErrorMessages.FeedPageCategoryHasPosts,
                Status = HttpStatusCode.Conflict
            });
        }

        var isCategoryExist = await _bll.FeedPageCategoryService.FindAsync(id);
        if (isCategoryExist == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        await _bll.FeedPageCategoryService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Create new Category with PageIdentifier
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> PostWithPageIdentifier(PostFeedPageCategoryWithPageIdentifier entity)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var feedPage = await _bll.FeedPageService.FindAsyncByName(entity.FeedPageIdentifier);
        if (feedPage == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var bllEntity = PostFeedPageCategoryWithPageIdenitiferMapper.Map(entity, contentTypes, feedPage.Id);
        var result = _bll.FeedPageCategoryService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get Categories without Posts
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/{identifier}")]
    public async Task<ActionResult<GetFeedPageCategoryWithoutPosts>> GetWithoutPosts(string languageCulture, string identifier)
    {
        var feedPage = await _bll.FeedPageService.FindAsyncByName(identifier);
        if (feedPage == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var result = await _bll.FeedPageCategoryService.GetCategoryWithoutPosts(feedPage.Id, languageCulture);
        
        var retrunResult = result.Select(e => GetFeedPageCategoryWithoutPostsMapper.Map(e, languageCulture)).ToList();
        return Ok(retrunResult);
    }
    

    /// <summary>
    /// Create new FeedPageCategory 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /*
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
    */

    /// <summary>
    /// Update Feed Page Category (PS. only Title!)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> Update(FeedPageCategory entity)
    {
        
        var isEntityNotExists = await _bll.FeedPageCategoryService.FindAsync(entity.Id) == null;
        if (isEntityNotExists)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.Conflict
            });
        }
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = FeedPageCategoryMapper.MapToUpdate(entity, contentTypes, entity.Id);
        var bllResult = await _bll.FeedPageCategoryService.UpdateAsync(bllEntity);
        var result = FeedPageCategoryMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return Ok(result);
    }
    
    /// <summary>
    /// Get Feed Page Post by id translated
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("/posts/{languageCulture}/{id}")]
    public async Task<ActionResult<Public.DTO.V1.FeedPage.FeedPageCategory>> Get(Guid id, string languageCulture)
    {
        var bllEntity = await _bll.FeedPageCategoryService.FindAsync(id, languageCulture);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = FeedPageCategoryMapper.Map(bllEntity, languageCulture); 
        return Ok(result);
    }
}
