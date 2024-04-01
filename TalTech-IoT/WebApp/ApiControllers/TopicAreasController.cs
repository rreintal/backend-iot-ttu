using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using App.DAL.EF.DbExceptions;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller regarding TopicAreas
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TopicAreasController : ControllerBase
{
    private readonly IAppBLL _bll;
    
    /// <inheritdoc />
    public TopicAreasController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Create TopicArea
    /// </summary>
    /// <param name="data"></param>
    /// <response code="409">TOPIC_AREA_CREATE_PARENT_DOES_NOT_EXIST, TOPIC_AREA_CREATE_NAME_EXISTS</response>
    /// <remarks>
    /// Sample request:
    ///
    /// {
    ///     "parentTopicId" : "parent-id",
    ///     "name" : [
    ///         {
    ///             "Value": "Robootika",
    ///             "Culutre": "et"
    ///         },
    ///         {
    ///             "Value": "Robotics",
    ///             "Culutre": "en"
    ///         }
    ///         ]
    /// }
    /// </remarks>
    /// <returns></returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Create([FromBody] PostTopicAreaDto data)
    {
        var bllEntity = CreateTopicAreaMapper.Map(data);
        try
        {
            var entity = _bll.TopicAreaService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            return Ok(new
            {
                Id = entity.Id
            });
        }
        catch (DbValidationExceptions myException)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "value already exists",
                Status = HttpStatusCode.Conflict
            });
        }
    }
    
    /// <summary>
    /// Get all TopicAreas
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}")]
    public async Task<IEnumerable<Public.DTO.V1.TopicArea>> Get(string languageCulture)
    {
        var items = (await _bll.TopicAreaService.AllAsync(languageCulture)).ToList();
        var result = TopicAreaMapper.Map(items);
        return result;
    }
    
    /// <summary>
    /// Get TopicAreas with both translations (EN/ET)
    /// </summary>
    /// <returns></returns>

    [HttpGet("[action]")]
    public async Task<IEnumerable<TopicAreaWithTranslation>> GetWithTranslation()
    {
        var items = (await _bll.TopicAreaService.AllAsync()).ToList();
        return TopicAreaWithTranslationMapper.Map(items);
    }

    /// <summary>
    /// Delete topic area
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.TopicAreaService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        try
        {
            _bll.TopicAreaService.Remove(entity);
            await _bll.SaveChangesAsync();

            return Ok();
        }
        catch (TopicAreaDeleteConstraintViolationException exception)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = RestApiErrorMessages.TopicAreaHasAssociatedNews
            });
        }
    }

    /// <summary>
    /// Get all Topic areas with count of how many news are associated
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/WithCount")]
    public async Task<IEnumerable<TopicAreaWithCount>> GetAllWithCount(string languageCulture)
    {
        var bllResult = await _bll.TopicAreaService.GetTopicAreasWithCount(languageCulture);

        return bllResult.Select(e => new TopicAreaWithCount()
        {
            Id = e.Id,
            Name = e.Name,
            Count = e.Count
        });
    }
}