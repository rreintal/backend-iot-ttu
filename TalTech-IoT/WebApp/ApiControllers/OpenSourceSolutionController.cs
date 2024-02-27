using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.V1.Mappers;
using Public.DTO.V1.OpenSourceSolution;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for OpenSourceSolutions
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class OpenSourceSolutionController : ControllerBase
{
    private readonly IAppBLL _bll;
    
    /// <summary>
    /// Controller for OpenSourceSolutions
    /// </summary>
    public OpenSourceSolutionController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Add new OpenSourceSolution
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<OpenSourceSolution>> Create([FromBody] OpenSourceSolution entity)
    {
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = OpenSourceSolutionMapper.Map(entity, types);
        var bllResult = _bll.OpenSourceSolutionService.Add(bllEntity);
        var result = OpenSourceSolutionMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Delete OpenSourceSolution by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.OpenSourceSolutionService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        _bll.OpenSourceSolutionService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get OpenSourceSolution by Id with all languages.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Preview/{id}")]
    public async Task<ActionResult<OpenSourceSolution>> Preview(Guid id)
    {
        var entity = await _bll.OpenSourceSolutionService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var result = OpenSourceSolutionMapper.Map(entity);
        return Ok(result);
    }

    /// <summary>
    /// Get OpenSourceSolution by languageCulture
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/{id}")]
    public async Task<ActionResult<GetOpenSourceSolution>> Get(Guid id, string languageCulture)
    {
        var entity = await _bll.OpenSourceSolutionService.FindAsync(id, languageCulture);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = OpenSourceSolutionMapper.Map(entity, languageCulture);
        return Ok(result);
    }
    
    /// <summary>
    /// Get all OpenSourceSolution by languageCulture
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}")]
    public async Task<ActionResult<ICollection<GetOpenSourceSolution>>> Get(string languageCulture)
    {
        var entityCollection = await _bll.OpenSourceSolutionService.AllAsync(languageCulture);
        var result = entityCollection.Select(e => OpenSourceSolutionMapper.Map(e, languageCulture));
        return Ok(result);
    }


    /// <summary>
    /// Update OpenSourceSolution
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] OpenSourceSolution entity)
    {
        var entityExists = await _bll.OpenSourceSolutionService.FindAsync(entity.Id) != null;
        if (!entityExists)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = OpenSourceSolutionMapper.MapToUpdate(entity, types, entity.Id);
        var bllResult = _bll.OpenSourceSolutionService.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get count of how many there are in total.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Count")]
    public async Task<ActionResult<int>> Count()
    {
        return await _bll.OpenSourceSolutionService.GetCount();
    }
    
}