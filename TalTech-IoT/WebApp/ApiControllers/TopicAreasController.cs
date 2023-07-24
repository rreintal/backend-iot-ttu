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
    public async Task<string> Create([FromBody] CreateTopicAreaDto data)
    {
        var bllEntity = CreateTopicAreaMapper.Map(data);
        var entity = _bll.TopicAreaService.Add(bllEntity);
        await _bll.SaveChangesAsync();
        return entity.Id.ToString();

    }
    
    // TODO - get all topics
    // with the amount of projects/news it has
    


}