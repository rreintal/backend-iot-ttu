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
/// Controller for Home Page Banners
/// </summary>
/// [ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class HomePageBannerController : ControllerBase
{

    private readonly IAppBLL _bll;

    /// <inheritdoc />
    public HomePageBannerController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Create new Home Page Banner
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<HomePageBanner> Add([FromBody] HomePageBanner data)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = HomePageBannerMapper.Map(data, contentTypes);
        var bllResult = _bll.HomePageBannerService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        var publicResult = HomePageBannerMapper.Map(bllResult);
        return publicResult;
    }

    /// <summary>
    /// Delete Home Page Banner by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.HomePageBannerService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        _bll.HomePageBannerService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get all Banners with all languages
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HomePageBanner>>> GetAll()
    {
        return (await _bll.HomePageBannerService.AllAsync()).Select(e => HomePageBannerMapper.Map(e)).ToList();
    }

    /// <summary>
    /// NOT WORKING, ISSUES WITH ALL OF UPDATE- needs refactoring!
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<HomePageBanner>> Update([FromBody] UpdateHomePageBanner entity)
    {
        var bllEntity = HomePageBannerMapper.Map(entity);
        var bllResult = await _bll.HomePageBannerService.Update(bllEntity);
        // TODO: actually not null?
        if (bllResult == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = HomePageBannerMapper.Map(bllResult);
        return Ok(result);
    }

    /// <summary>
    /// Get all banners by language
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}")]
    public async Task<IEnumerable<GetHomePageBanner>> GetAllByLanguageCulture(string languageCulture)
    {
        return (await _bll.HomePageBannerService.AllAsync(languageCulture)).Select(e => GetHomePageBannerMapper.Map(e));
    }
    
    /// <summary>
    /// Get banner by Id with both languages
    /// </summary>
    /// <returns></returns>
    [HttpGet("Preview/{id}")]
    public async Task<ActionResult<HomePageBanner>> GetPreview(Guid id)
    {
        var bllEntity = await _bll.HomePageBannerService.FindAsync(id);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        return HomePageBannerMapper.Map(bllEntity);
    }
}