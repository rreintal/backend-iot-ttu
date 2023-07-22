using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1;

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
    public async Task<TopicArea> Create([FromBody] CreateTopicAreaDto data)
    {
        
        throw new NotImplementedException();
    }
    
    
}