using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;


/// <summary>
/// Projects controller
/// </summary>
//[Route("api/{languageCulture}/[controller]/[action]")]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private IAppBLL _bll;

    /// <inheritdoc />
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Public.DTO.V1.PostProjectSuccessDto>> Create([FromBody] Public.DTO.V1.PostProjectDto data)
    {
        var types = await _bll.NewsService.GetContentTypes(); // TODO - tee eraldi service ehk?
        var bllEntity = ProjectMapper.Map(data, types);
         var addedEntity = _bll.ProjectService.Add(bllEntity);
         await _bll.SaveChangesAsync();
         var result = ProjectMapper.Map(addedEntity);

        return Ok(result);
    }

    /// <summary>
    /// Get Projects
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    /// 
    [HttpGet("{languageCulture}")]
    public async Task<IEnumerable<GetProject>> Get(string languageCulture)
    {
        return (await _bll.ProjectService.AllAsync(languageCulture)).Select(x => GetProjectMapper.Map(x));
    }

    /// <summary>
    /// ProcessDelete Project
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.ProjectService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        await _bll.ProjectService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return Ok(new RestApiResponse()
        {
            Message = $"Deleted project with id {id.ToString()}",
            Status = HttpStatusCode.OK
        });
    }
    
    /// <summary>
    /// Get Projects count
    /// </summary>
    /// <returns></returns>
    [HttpGet("Count")]
    public async Task<int> GetTotalProjectsCount()
    {
        return await _bll.ProjectService.FindProjectTotalCount();
    }

    /// <summary>
    /// Get Project by id
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/{id}")]
    public async Task<ActionResult<GetProject>> Get(string languageCulture, Guid id)
    {
        var entity = await _bll.ProjectService.FindAsync(id, languageCulture);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        await IncreaseViewCount(id);
        var result = GetProjectMapper.Map(entity);
        return result;
    }
    
    private async Task IncreaseViewCount(Guid id)
    {
        var isClientHeaderPresent = HttpContext.Request.Headers.ContainsKey("IOT-App");
        if (isClientHeaderPresent)
        {
            await _bll.ProjectService.IncrementViewCount(id);
            await _bll.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Update project
    /// </summary>
    /// <param name="languageCulture"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Update([FromBody] UpdateProject data)
    {
        var contentTypes = await _bll.NewsService.GetContentTypes();
        var bllEntity = UpdateProjectMapper.Map(data, contentTypes);
        var result = await _bll.ProjectService.UpdateAsync(bllEntity);

        if (result == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        await _bll.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Returns project with all the language cultures
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Preview/{id}")]
    public async Task<ActionResult<Public.DTO.V1.ProjectAllLangs>> GetProjectAllLanguages(Guid id)
    {
        var entity = await _bll.ProjectService.FindByIdAsyncAllLanguages(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        return ProjectAllLangsMapper.Map(entity);
    }

    [HttpPost("Ongoing")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> ToggleProjectStatus(Guid id, bool isOngoing)
    {
        var result = await _bll.ProjectService.ChangeProjectStatus(id, isOngoing);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

}