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
    private IAppBLL _bll;
    
    /// <inheritdoc />
    public FeedPageController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<ActionResult<Public.DTO.V1.FeedPage.FeedPage>> Post(Public.DTO.V1.FeedPage.FeedPage entity)
    {
        var bllEntity = FeedPageMapper.Map(entity);
        var result  = _bll.FeedPageService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<Public.DTO.V1.FeedPage.FeedPage>> Get(string identifier)
    {
        var bllEntity = await _bll.FeedPageService.FindAsyncByName(identifier);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        return Ok(FeedPageMapper.Map(bllEntity));
    }
}