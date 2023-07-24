using App.BLL.Contracts;
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

    public TopicAreasController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicAreaDto data)
    {
        var bllEntity = CreateTopicAreaMapper.Map(data);
        var entity = _bll.TopicAreaService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok(new
        {
            TopicAreaId = entity.Id.ToString()
        });
    }

    [HttpGet]
    public async Task<IEnumerable<Public.DTO.V1.TopicArea>> Get(string languageCulture)
    {
        // TODO - filtering with the amount of projects/news it has
        
        _bll.TopicAreaService.SetLanguageStrategy(languageCulture);
        var items = (await _bll.TopicAreaService.AllAsync()).Select(e => ReturnTopicAreaMapper.Map(e));
        return items;
    }

    
    


}