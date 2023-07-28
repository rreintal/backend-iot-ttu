using App.BLL.Contracts;
using AutoMapper;
using DAL.DTO.V1.FilterObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller regarding TopicAreas
/// </summary>
[Route("api/{languageCulture}/[controller]/[action]")]
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
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostTopicAreaDto data, string languageCulture)
    {
        // TODO - if db throws error, then show it to the user!
        var bllEntity = CreateTopicAreaMapper.Map(data);
        var entity = _bll.TopicAreaService.Add(bllEntity);
        try
        {
            await _bll.SaveChangesAsync();
        }
        
        // TODO - if Topic Area with that name already exists, throws that error!
        catch (DbUpdateException e)
        {
            return BadRequest(new
            {
                Error = $"TopicArea with name '{data.Name[0].Value}' for culture '{languageCulture}' already exists.",
                Code = "TOPICAREA_EXISTS"
            });
        }
        
        return Ok(new
        {
            TopicAreaId = entity.Id.ToString()
        });
    }
    /// <summary>
    /// Get all TopicAreas
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<Public.DTO.V1.TopicArea>> Get(string languageCulture)
    {
        // TODO - filtering with the amount of projects/news it has
        _bll.TopicAreaService.SetLanguageStrategy(languageCulture);
        var items = (await _bll.TopicAreaService.AllAsync()).ToList();
        
        var result = TopicAreaMapper.Map(items);
        return result;
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
        _bll.TopicAreaService.SetLanguageStrategy(languageCulture);
        
        var filter = new TopicAreaCountFilter()
        {
            News = News,
            Projects = Projects
        };
        
        var result = await _bll.TopicAreaService.GetTopicAreaWithCount(filter);
        return result.Select(e => TopicAreaWithCountMapper.Map(e));
    }
}