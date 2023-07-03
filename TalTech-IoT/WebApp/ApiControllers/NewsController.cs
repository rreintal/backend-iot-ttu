using System.Net.Mime;
using App.DAL.EF;
using App.Domain;
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

    [HttpPost]
    public async Task<string> Create([FromBody] DTO.V1.NewsDTO payload)
    {
        
        // TODO mapper
        var newsId = Guid.NewGuid();
        var bodyContentType = _context.ContentTypes.Where(x => x.Name == "BODY").First();
        var titleContentType = _context.ContentTypes.Where(x => x.Name == "TITLE").First();
        
        var estTitle = payload.Title.First(x => x.Culture == LanguageCulture.EST);
        var estBody = payload.Body.First(x => x.Culture == LanguageCulture.EST);


        var bodyLangStrID = Guid.NewGuid();
        var bodyLangStr = new LanguageString()
        {
            Id = bodyLangStrID,
            Value = estBody.Value
        };
        var bodyContent = new Content()
        {
            NewsId = newsId,
            ContentTypeId = bodyContentType.Id,
            LanguageStringId = bodyLangStrID,
            LanguageString = bodyLangStr
        };

        bodyLangStr.Content = bodyContent;

        var titleLangStrId = Guid.NewGuid();
        var titleLangStr = new LanguageString()
        {
            Id = titleLangStrId,
            Value = estTitle.Value
        };

        var titleContent = new Content()
        {
            NewsId = newsId,
            ContentTypeId = titleContentType.Id,
            LanguageStringId = titleLangStrId,
            LanguageString = titleLangStr
        };
        titleLangStr.Content = titleContent;

        var bodyTranslations = new List<LanguageStringTranslation>();
        foreach (var bodyDto in payload.Body)
        {
            var langStr = new LanguageStringTranslation()
            {
                LanguageCulture = bodyDto.Culture,
                TranslationValue = bodyDto.Value,
                LanguageStringId = bodyLangStr.Id
            };
            bodyTranslations.Add(langStr);
        }
        var titleTranslations = new List<LanguageStringTranslation>();
        foreach (var titleDto in payload.Title)
        {
            var langStr = new LanguageStringTranslation()
            {
                LanguageCulture = titleDto.Culture,
                TranslationValue = titleDto.Value,
                LanguageStringId = titleLangStr.Id
            };
            titleTranslations.Add(langStr);
        }

        titleLangStr.LanguageStringTranslations = titleTranslations;
        bodyLangStr.LanguageStringTranslations = bodyTranslations;

        var news = new News()
        {
            Content = new List<Content>()
            { 
                titleContent, bodyContent
            },
        };

        var res = (await _context.News.AddAsync(news)).Entity;
        await _context.SaveChangesAsync();
        return res.Id.ToString();
    }

    [HttpGet]
    public async Task<DTO.V1.News> GetById(Guid id, string languageCulture)
    {
        var query = await _context.News.Where(x => x.Id == id)
            .Include(x => x.Content)
            .ThenInclude(x => x.ContentType)
            .Include(x => x.Content)
            .ThenInclude(x => x.LanguageString)
            .ThenInclude(x => x.LanguageStringTranslations)
            .FirstAsync();

        var title = query.Content.First(x => x.ContentType!.Name == "TITLE")
            .LanguageString
            .LanguageStringTranslations.First(x => x.LanguageCulture == languageCulture).TranslationValue;
        
        var body = query.Content.First(x => x.ContentType!.Name == "TITLE")
            .LanguageString
            .LanguageStringTranslations.First(x => x.LanguageCulture == languageCulture).TranslationValue;

        var res = new DTO.V1.News()
        {
            Id = query.Id,
            Title = title,
            Body = body,
            CreatedAt = query.CreatedAt
        };

        return res;
    }
    
    
    [HttpPost]
    public async Task<int> AddContentType([FromBody] CreateContentDTO data)
    {
        Console.WriteLine($"Typename: {data.TypeName}");
        
        var res = new App.Domain.ContentType()
        {
            Name = data.TypeName
        };
        await _context.ContentTypes.AddAsync(res);
        return await _context.SaveChangesAsync();
    }

    public class CreateContentDTO
    {
        public string TypeName { get; set; } = default!;
    }
}