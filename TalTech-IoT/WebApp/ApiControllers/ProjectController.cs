using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;


/// <summary>
/// Projects controller
/// </summary>
[Route("api/{languageCulture}/[controller]/[action]")]
public class ProjectController : ControllerBase
{
    private IAppBLL _bll;

    public ProjectController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Add new Project
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Public.DTO.V1.PostProjectDto data)
    {
        // TODO - tee eraldi service ehk?
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = ProjectMapper.Map(data, types);
        var result = _bll.ProjectService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        return Ok(new
        {
            ProjectId = result.Id
        });
    }
}