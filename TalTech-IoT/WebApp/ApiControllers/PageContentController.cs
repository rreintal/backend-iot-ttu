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
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PageContentController : ControllerBase
{
    private IAppBLL _bll;

    /// <inheritdoc />
    public PageContentController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<PageContent> Add(PageContent entity)
    {
        // TODO: entity.PageIdentifier is the ID!
        var contentTypes = await _bll.NewsService.GetContentTypes();
        //var mappedEntity = CreatePageContentMapper.MapHack(entity, contentTypes);
        var mappedEntity = CreatePageContentMapper.Map(entity, contentTypes);
        _bll.PageContentService.Add(mappedEntity);
        await _bll.SaveChangesAsync();
        return entity;
    }
    
    
    // TODO: see oleks nagu Preview!
    [HttpGet("{pageIdentifier}")]
    public async Task<ActionResult<App.Domain.PageContent?>> Get(string pageIdentifier)
    {
        var domainObject = await _bll.PageContentService.FindAsyncByIdentifierString(pageIdentifier);
        if (domainObject == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.GeneralNotFound
            });
        }

        var result = GetPageContentMapper.MapBLL(domainObject);
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
        var domainObject = await _bll.PageContentService.FindAsyncByIdentifierString(pageIdentifier, languageCulture);
        if (domainObject == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.GeneralNotFound
            });
        }

        //var result = GetPageContentMapper.MapTemporaryHack(domainObject, languageCulture);
        var result = GetPageContentMapper.MapBLL(domainObject);
        return Ok(result);
        
    }
    
    /// <summary>
    /// WORKING WITH HACK!
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="pageIdentifier"></param>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{languageCulture}/{pageIdentifier}")]
    public async Task<ActionResult> Update(string languageCulture, string pageIdentifier, PageContent content)
    {
        
        var contentTypes = await _bll.NewsService.GetContentTypes();
        //var mappedEntity = CreatePageContentMapper.MapHack(content, contentTypes);
        var mappedEntity = CreatePageContentMapper.Map(content, contentTypes);

        // TODO: HACK!
        _bll.PageContentService.Update(mappedEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }
    
    
}