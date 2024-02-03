using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using App.DAL.EF.DbExceptions;
using Asp.Versioning;
using AutoMapper;
using DAL.DTO.V1.FilterObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.ApiExceptions;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;
using TopicArea = BLL.DTO.V1.TopicArea;

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
    private readonly IMapper _mapper;

    public TopicAreasController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        this._mapper = mapper;
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
    /// Get TopicAreas based on filter.
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="News"></param>
    /// <param name="Projects"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<TopicAreaWithCount>> GetWithCount(string languageCulture, bool? News, bool? Projects)
    {
        var filter = new TopicAreaCountFilter()
        {
            News = News,
            Projects = Projects
        };
        
        var result = await _bll.TopicAreaService.GetTopicAreaWithCount(filter, languageCulture);
        return result.Select(e => TopicAreaWithCountMapper.Map(e));
    }
}