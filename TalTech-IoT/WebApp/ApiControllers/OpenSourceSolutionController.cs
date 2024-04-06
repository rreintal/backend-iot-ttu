using System.Net;
using App.BLL.Contracts;
using App.DAL.EF;
using App.Domain;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.V1.Mappers;
using Public.DTO.V1.OpenSourceSolution;
using AccessDetails = BLL.DTO.V1.AccessDetails;
using OpenSourceSolution = Public.DTO.V1.OpenSourceSolution.OpenSourceSolution;

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
    private readonly AppDbContext _context;
    
    /// <summary>
    /// Controller for OpenSourceSolutions
    /// </summary>
    public OpenSourceSolutionController(IAppBLL bll, AppDbContext context)
    {
        _bll = bll;
        _context = context;
    }

    /// <summary>
    /// Add new OpenSourceSolution
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    /// ProcessDelete OpenSourceSolution by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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


    /// <summary>
    /// Access OSS link via Mail
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost("{languageCulture}/RequestAccess")]
    public async Task<bool> GetResource(RequestOpenSourceSolutionAccess data, string languageCulture)
    {
        // TODO: Move this to Mail controller!!
        var openSourceSolution = await _bll.OpenSourceSolutionService.FindAsync(data.SolutionId);
        
        // TODO: tee eraldi DTO + meetod selle jaoks
        var titleName = openSourceSolution!.Content.First(x => x.ContentType!.Name == "TITLE");
        var name = titleName.LanguageString.LanguageStringTranslations.First().TranslationValue;
        _bll.MailService.AccessResource(data.Email, name, openSourceSolution.Link, languageCulture);
        
        // TODO: lisa alles siis kui mail on successful!
        
        var bllEntity = AccessDetailsMapper.Map(data);
        _bll.AccessDetailsService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        return true;
    }
    
    [HttpGet("{languageCulture}/RequestInfo")]
    public async Task<List<OpenSourceSolutionRequestInfo>> GetWithAccessDetails(string languageCulture)
    {
        var items = await _bll.OpenSourceSolutionService.AllAsyncWithStatistics();
        return items.Select(e => OpenSourceSolutionWithStatisticsMapper.Map(e, languageCulture)).ToList();
    }
}