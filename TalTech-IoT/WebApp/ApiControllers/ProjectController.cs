using System.Net;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
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

    /// <summary>
    /// Get Projects
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<GetProject>> Get(string languageCulture)
    {
        _bll.ProjectService.SetLanguageStrategy(languageCulture);
        return (await _bll.ProjectService.AllAsync()).Select(x => GetProjectMapper.Map(x, true));
    }

    /// <summary>
    /// Delete Project
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.ProjectService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = "Project with this id does not exist.",
                StatusCode = (int)HttpStatusCode.NotFound
            });
        }
        _bll.ProjectService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok(new RestApiResponse()
        {
            Message = $"Deleted project with id {id.ToString()}",
            StatusCode = (int)HttpStatusCode.OK
        });
    }

    /// <summary>
    /// Get Project by id
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<GetProject>> GetById(string languageCulture, Guid id)
    {
        _bll.ProjectService.SetLanguageStrategy(languageCulture);
        var entity = await _bll.ProjectService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = "Project with this id does not exist.",
                StatusCode = (int)HttpStatusCode.NotFound
            });
        }

        var result = GetProjectMapper.Map(entity);
        return result;
    }
    
}