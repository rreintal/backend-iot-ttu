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
//[Route("api/{languageCulture}/[controller]/[action]")]
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
    [HttpPost("api/[controller]/")]
    public async Task<ActionResult> Create([FromBody] Public.DTO.V1.PostProjectDto data)
    {
        // TODO - image optionaliks!
        
        // TODO: TopicArea must only have Id, other is optional!
        
        var types = await _bll.NewsService.GetContentTypes(); // TODO - tee eraldi service ehk?
        
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
    /// 
    [HttpGet("api/{languageCulture}/[controller]/")]
    public async Task<IEnumerable<GetProject>> Get(string languageCulture)
    {
        return (await _bll.ProjectService.AllAsync(languageCulture)).Select(x => GetProjectMapper.Map(x, true));
    }

    /// <summary>
    /// Delete Project
    /// </summary>
    /// <returns></returns>
    [HttpDelete("api/[controller]/{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.ProjectService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Error = "Project with this id does not exist.",
                Status = HttpStatusCode.NotFound
            });
        }
        _bll.ProjectService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok(new RestApiResponse()
        {
            Error = $"Deleted project with id {id.ToString()}",
            Status = HttpStatusCode.OK
        });
    }

    /// <summary>
    /// Get Project by id
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("api/{languageCulture}/[controller]/{id}")]
    public async Task<ActionResult<GetProject>> Get(string languageCulture, Guid id)
    {
        //var entity = await _bll.ProjectService.FindAsync(id, languageCulture);
        var entity = await _bll.ProjectService.FindAsync(id, languageCulture);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Error = "Project with this id does not exist.",
                Status = HttpStatusCode.NotFound
            });
        }

        var result = GetProjectMapper.Map(entity);
        return result;
    }
    
}