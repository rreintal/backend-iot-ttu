using App.Domain;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for API languages
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class LanguageController : ControllerBase
{
    
    /// <summary>
    /// Returns all language cultures which are supported by API.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<string> GetSupportedLanguages()
    {
        
        return LanguageCulture.ALL_LANGUAGES;
    }
}