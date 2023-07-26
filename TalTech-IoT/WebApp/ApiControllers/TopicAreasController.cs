using App.BLL.Contracts;
using AutoMapper;
using DAL.DTO.V1.FilterObjects;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Create([FromBody] PostTopicAreaDto data)
    {
        // TODO - if db throws error, then show it to the user!
        var bllEntity = CreateTopicAreaMapper.Map(data);
        var entity = _bll.TopicAreaService.Add(bllEntity);
        await _bll.SaveChangesAsync();
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
        return TopicAreaMapper.Map(items);
    }

    
    [Obsolete("Not working rn")]
    [HttpGet]
    public async Task<IEnumerable<TopicAreaWithCount>> GetWithCount(string languageCulture, bool News, bool? Projects)
    {
        _bll.TopicAreaService.SetLanguageStrategy(languageCulture);
        
        var filter = new TopicAreaCountFilter()
        {
            News = News,
            Projects = News
        };
        
        
        // TODO -
        // kui on
        // * Programming
        //   * Java
        
        // Siis result tuleb ainult Java (1)
        // 
        // Aga peaks tulema Programming (1), Java(1)

        var result = await _bll.TopicAreaService.GetTopicAreaWithCount(filter);
        return result.Select(e => TopicAreaWithCountMapper.Map(e));
    }
    

    
    


}