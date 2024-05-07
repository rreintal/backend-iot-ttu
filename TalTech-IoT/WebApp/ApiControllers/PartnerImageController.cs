using System.Net;
using App.BLL.Contracts;
using App.BLL.Mappers;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
using PartnerImageMapper = Public.DTO.V1.Mappers.PartnerImageMapper;

namespace WebApp.ApiControllers;


/// <summary>
/// Controller for PartnerImage
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PartnerImageController : ControllerBase
{
    private readonly IAppBLL _bll;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bll"></param>
    public PartnerImageController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Add new PartnerImage
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<PartnerImage>> Add([FromBody] PartnerImage data)
    {
        var bllEntity = PartnerImageMapper.Map(data);
        var bllResult = _bll.PartnerImageService.Add(bllEntity);
        var result = PartnerImageMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return result;
    }


    /// <summary>
    /// Get PartnerImage by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /*
    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerImage>> Get(Guid id)
    {
        var entity = await _bll.PartnerImageService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        return PartnerImageMapper.Map(entity);
    }
    */

    /// <summary>
    /// ProcessDelete PartnerImage by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete(Guid id)
    {
        // TODO: sequence number unique?
        
        var entity = await _bll.PartnerImageService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        await _bll.PartnerImageService.RemoveAsync(entity.Id);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get all PartnerImages
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<PartnerImage>> GetAll()
    {
        var result = await _bll.PartnerImageService.AllAsync();
        return result.Select(e => PartnerImageMapper.Map(e));
    }
}