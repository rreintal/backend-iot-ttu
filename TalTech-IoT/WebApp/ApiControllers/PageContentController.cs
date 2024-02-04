using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for page content
/// </summary>
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PageContentController : ControllerBase
{
    private IAppBLL _bll;

    /// <inheritdoc />
    public PageContentController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<ActionResult<PageContent>> Add(PageContent entity)
    {
        var isUnique = await _bll.PageContentService.FindAsyncByIdentifierString(entity.PageIdentifier) == null;
        if (!isUnique)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.AlreadyExists,
                Status = HttpStatusCode.BadRequest
            });
        }
        
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var mappedEntity = CreatePageContentMapper.Map(entity, contentTypes);
        _bll.PageContentService.Add(mappedEntity);
        await _bll.SaveChangesAsync();
        return entity;
    }
    
    
    // TODO: see oleks nagu Preview!
    [HttpGet("{pageIdentifier}")]
    public async Task<ActionResult<Public.DTO.V1.PageContent?>> Get(string pageIdentifier)
    {
        var bllObject = await _bll.PageContentService.FindAsyncByIdentifierString(pageIdentifier);
        if (bllObject == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.GeneralNotFound
            });
        }

        var result = PageContentMapper.Map(bllObject);
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="pageIdentifier"></param>
    [HttpGet("{languageCulture}/{pageIdentifier}")]
    public async Task<ActionResult<Public.DTO.V1.GetPageContent?>> Get(string languageCulture, string pageIdentifier)
    {
        var bllObject = await _bll.PageContentService.FindAsyncByIdentifierString(pageIdentifier, languageCulture);
        if (bllObject == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.GeneralNotFound
            });
        }
        var result = PageContentMapper.MapTemporaryHack(bllObject, languageCulture);
        return Ok(result);
        
    }
    
    /// <summary>
    /// WORKING WITH HACK!
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="pageIdentifier"></param>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{pageIdentifier}")]
    public async Task<ActionResult> Update(string pageIdentifier, PageContent content)
    {
        
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var mappedEntity = CreatePageContentMapper.Map(content, contentTypes);

        // TODO: HACK!
        await _bll.PageContentService.UpdateAsync(mappedEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }
    
    
}