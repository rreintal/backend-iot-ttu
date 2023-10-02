using App.BLL.Contracts;
using AutoMapper;
using DAL.DTO.V1.FilterObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.ApiExceptions;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;
using TopicArea = BLL.DTO.V1.TopicArea;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller regarding TopicAreas
/// </summary>
//[Route("api/{languageCulture}/[controller]/[action]")]
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
    [HttpPost("api/[controller]/")]
    public async Task<IActionResult> Create([FromBody] PostTopicAreaDto data)
    {
        var bllEntity = CreateTopicAreaMapper.Map(data);
        var entity = _bll.TopicAreaService.Add(bllEntity);
        try
        {
            await _bll.SaveChangesAsync();
        }
        
        // If Topic Area with that name already exists then it throws that error!
        // All Topic Areas must be with unique name.
        catch (DbUpdateException e)
        {
            throw new ValueAlreadyExistsException(entity.GetName());
        }

        return Ok(new
        {
            Id = entity.Id
        });
    }
    /// <summary>
    /// Get all TopicAreas
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("api/{languageCulture}/[controller]/")]
    public async Task<IEnumerable<Public.DTO.V1.TopicArea>> Get(string languageCulture)
    {
        _bll.TopicAreaService.SetLanguageStrategy(languageCulture);
        var items = (await _bll.TopicAreaService.AllAsync()).ToList();
        var result = TopicAreaMapper.Map(items);
        return result;
    }
    
    /// <summary>
    /// Get TopicAreas with both translations (EN/ET)
    /// </summary>
    /// <returns></returns>

    [HttpGet("api/[controller]/[action]")]
    public async Task<IEnumerable<TopicAreaWithTranslation>> GetWithTranslation()
    {
        // TODO - create an object where you store list of languageCultures as method parameter {languageStringList}
        // overkill?
        
        //var items = (await _bll.TopicAreaService.AllAsync()).ToList();
        var items = (await _bll.TopicAreaService.GetTopicAreasWithTranslations()).ToList();

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