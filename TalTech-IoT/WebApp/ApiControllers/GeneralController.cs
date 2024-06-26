using App.Domain;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

/// <summary>
/// General App info
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class GeneralController : ControllerBase
{
    /// <inheritdoc />
    public GeneralController()
    {
    }
    
    /// <summary>
    /// Returns all language cultures which are supported by API.
    /// </summary>
    /// <returns></returns>
    [HttpGet("languages")]
    public IEnumerable<string> GetSupportedLanguages()
    {
        
        return LanguageCulture.ALL_LANGUAGES;
    }

    /// <summary>
    /// Endpoint to check application status.
    /// </summary>
    /// <returns></returns>
    [HttpGet("status")]
    public ActionResult Status()
    {
        return Ok();
    }
}