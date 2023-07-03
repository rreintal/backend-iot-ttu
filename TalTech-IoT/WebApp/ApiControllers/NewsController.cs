using App.DAL.EF;
using App.Domain;
using App.Domain.Constants;
using App.Domain.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
/// 
/// </summary>
//[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Route("api/{languageCulture}/[controller]/[action]")]
public class NewsController : ControllerBase
{
    protected AppDbContext _context;

    public NewsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<DTO.V1.News?> GetNewsById(Guid id, string languageCulture)
    {
        /*var query = await _context.News.Where(x => x.Id == id)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageStringTranslations)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageStringType)
            .FirstAsync();
        */
        var news = await _context.News.Where(x => x.Id == id)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageStringTranslations)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageStringType)
            .FirstAsync();
        
        var titleStr = news.Content.Where(x => x.LanguageStringType!.Name == "TITLE")
            .First().LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture).First().TranslationValue;
        var bodyStr = news.Content.Where(x => x.LanguageStringType!.Name == "BODY")
            .First().LanguageStringTranslations.Where(x => x.LanguageCulture == languageCulture).First().TranslationValue;

        var res = new DTO.V1.News()
        {
            Body = bodyStr,
            Id = news.Id,
            Title = titleStr,
            CreatedAt = news.CreatedAt
        };
        return res;
    }

    [HttpPost]
    public async Task<string> CreateNews([FromBody] DTO.V1.NewsDTO newsDTO)
    {
        Console.WriteLine(newsDTO.Body);
        // TODO - see juhtub BLL-is
        
        var newsId = Guid.NewGuid();
        // CREATE LANGUAGE STRING TYPE SERVICE, WHICH LOADS THEM TO MEMORY ON STARTUP!!!
        var titleType = _context.LanguageStringTypes.First(x => x.Name == LanguageStringTypeConstants.TITLE);
        var bodyType = _context.LanguageStringTypes.First(x => x.Name == LanguageStringTypeConstants.BODY);
        
        // create languageStrings (body)
        var body = newsDTO.Body.First(x => x.Culture == LanguageCulture.EST);
        var bodyLangStr = new LanguageString()
        {
            Value = body.Value,
            LanguageStringTypeId = bodyType.Id,
            NewsId = newsId
        };
        
        var title = newsDTO.Title.First(x => x.Culture == LanguageCulture.EST);
        var titleLangStr = new LanguageString()
        {
            Value = title.Value,
            LanguageStringTypeId = titleType.Id,
            NewsId = newsId
        };
        
        

        var Bodytranslations = new List<LanguageStringTranslation>();
        foreach (var bodyDTO in newsDTO.Body)
        {
            var languageStrTranslation = new LanguageStringTranslation()
            {
                LanguageCulture = bodyDTO.Culture,
                LanguageStringId = bodyLangStr.Id,
                TranslationValue = bodyDTO.Value
            };
            Bodytranslations.Add(languageStrTranslation);
        }

        var titleTranslations = new List<LanguageStringTranslation>();
        foreach (var bodyDTO in newsDTO.Title)
        {
            var languageStrTranslation = new LanguageStringTranslation()
            {
                LanguageCulture = bodyDTO.Culture,
                LanguageStringId = titleLangStr.Id,
                TranslationValue = bodyDTO.Value
            };
            titleTranslations.Add(languageStrTranslation);
        }

        titleLangStr.LanguageStringTranslations = titleTranslations;
        bodyLangStr.LanguageStringTranslations = Bodytranslations;

        var news = new News()
        {
            Content = new List<LanguageString>()
            {
                titleLangStr, bodyLangStr
            }
        };
        news = _context.News.Add(news).Entity;
        _context.SaveChanges();
        Console.WriteLine("Saved news!");
        return news.Id.ToString();
    }

    [HttpPost]
    public async Task<ActionResult> AddType([FromBody] string TypeName)
    {
        Console.WriteLine($"Adding type named: {TypeName}");
        var type = new LanguageStringType()
        {
            Name = TypeName
        };
        await  _context.LanguageStringTypes.AddAsync(type);
        Console.WriteLine($"Added type: {TypeName}");
        await _context.SaveChangesAsync();
        return Ok();
    }
}