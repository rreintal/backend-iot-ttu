using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

public class DevelopController : ControllerBase
{
    protected AppDbContext _context;
    
    public DevelopController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost("dev/langstr")]
    public async Task AddLangStr([FromBody] LangStrDTO data)
    {
        var langStr = new LanguageString()
        {
            Value = data.EstValue
        };

        var langStrTranslationEng = new LanguageStringTranslation()
        {
            LanguageStringId = langStr.Id,
            TranslationValue = data.EngValue,
            LanguageCulture = "eng"
        };

        var langStrTranslationEst = new LanguageStringTranslation()
        {
            LanguageStringId = langStr.Id,
            TranslationValue = data.EstValue,
            LanguageCulture = "est"
        };

        langStr.LanguageStringTranslations = new List<LanguageStringTranslation>()
        {
            langStrTranslationEng,
            langStrTranslationEst
        };

        _context.Add(langStr);
        await _context.SaveChangesAsync();
        Console.WriteLine("Added!");
    }
    [HttpPost("dev/translate")]
    public async Task<string> GetTranslation([FromBody] GetTranslationDTO data)
    {
        
        var currentWordInTranslationTable = await _context.LanguageStringTranslations
            .Include(x => x.LanguageString)
            .Where(x => x.TranslationValue == data.Value)
            .FirstAsync();
        
        var translationStringId = currentWordInTranslationTable.LanguageString!.Id;

        var result = await _context.LanguageStringTranslations
            .Include(x => x.LanguageString)
            .Where(translationObject =>
                translationObject.LanguageStringId == translationStringId &&
                translationObject.LanguageCulture == data.Culture)
            .FirstAsync();

        return result.TranslationValue;
    }
}

public class GetTranslationDTO
{
    public string Value { get; set; } = default!;
    public string Culture { get; set; } = default!;
}
public class LangStrDTO
{
    public string EngValue { get; set; } = default!;
    public string EstValue { get; set; } = default!;
}
