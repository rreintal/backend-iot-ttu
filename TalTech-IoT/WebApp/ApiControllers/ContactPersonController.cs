using System.Net;
using App.BLL.Contracts;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;
using Public.DTO.Identity;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class ContactPersonController : ControllerBase
{
    private readonly IAppBLL _bll;

    /// <inheritdoc />
    public ContactPersonController(IAppBLL bll)
    {
        _bll = bll;
    }

    /// <summary>
    /// Add new contact person
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Public.DTO.V1.ContactPerson>> Add([FromBody] Public.DTO.V1.ContactPerson data)
    {
        var types = await _bll.NewsService.GetContentTypes();
        var bllEntity = ContactPersonMapper.Map(data, types);
        var bllResult = _bll.ContactPersonService.Add(bllEntity);
        var result = ContactPersonMapper.Map(bllResult);
        await _bll.SaveChangesAsync();
        return result;
    }

    /// <summary>
    /// Delete contact person by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var entity = await _bll.ContactPersonService.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        _bll.ContactPersonService.Remove(entity);
        await _bll.SaveChangesAsync();
        return Ok();
    }
    
    
    /// <summary>
    /// Preview
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Preview/{id}")]
    public async Task<ActionResult<ContactPerson>> GetAllLanguages(Guid id)
    {
        var bllEntity = await _bll.ContactPersonService.FindAsync(id);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        return GetContactPersonsAllLanguagesMapper.Map(bllEntity);
    }
    
    /// <summary>
    /// Get Contact person based on language
    /// </summary>
    /// <param name="id"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpGet("{languageCulture}/{id}")]
    public async Task<ActionResult<Public.DTO.V1.GetContactPerson>> Get(Guid id, string languageCulture)
    {
        var bllEntity = await _bll.ContactPersonService.FindAsync(id, languageCulture);
        if (bllEntity == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        var result = GetContactPersonMapper.Map(bllEntity);
        return result;
    }
    
    [HttpGet("{languageCulture}")]
    public async Task<ActionResult<IEnumerable<Public.DTO.V1.GetContactPerson>>> GetAll(string languageCulture)
    {
        var bllResult = await _bll.ContactPersonService.AllAsync(languageCulture);
        var result = bllResult.Select(e => GetContactPersonMapper.Map(e));
        return Ok(result);
    }
    
}