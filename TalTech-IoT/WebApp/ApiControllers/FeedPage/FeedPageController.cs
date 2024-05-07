using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers.FeedPage;


/// <summary>
/// Controller for FeedPage
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class FeedPageController : ControllerBase
{
    private readonly IAppBLL _bll;
    
    /// <inheritdoc />
    public FeedPageController(IAppBLL bll)
    {
        _bll = bll;
    }
    
    /// <summary>
    /// Get Feed Page with Posts and Categories translated
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/{identifier}")]
    public async Task<ActionResult<Public.DTO.V1.FeedPage.GetFeedPageTranslated>> Get(string identifier, string languageCulture)
    {
        var bllEntity = await _bll.FeedPageService.FindAsyncByNameTranslated(identifier, languageCulture);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = FeedPageMapper.MapTranslated(bllEntity, languageCulture); 
        return Ok(result);
    }
    
    
    
}