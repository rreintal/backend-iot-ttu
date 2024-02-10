using App.Domain.Contracts;

namespace App.Domain.Helpers;

public static class UpdateContentHelper
{
    public static void UpdateContent(IContentEntity existingEntity, IContentEntity newEntity)
    {
        var cults = LanguageCulture.ALL_LANGUAGES;
        foreach (var lang in cults)
        {
            var newBodyValue = GetContentValue(newEntity, ContentTypes.BODY, lang);
            var newTitleValue = GetContentValue(newEntity, ContentTypes.TITLE, lang);
    
            var oldBodyValue = GetContentValue(existingEntity, ContentTypes.BODY, lang);
            var oldTitleValue = GetContentValue(existingEntity, ContentTypes.TITLE, lang);

            var isBodyValueChanged = oldBodyValue != newBodyValue;
            var isTitleContentChanged = oldTitleValue != newTitleValue;
            
            if (isBodyValueChanged)
            {
                SetContentTranslationValue(existingEntity, ContentTypes.BODY, lang, newBodyValue);
                SetBaseLanguage(existingEntity, ContentTypes.BODY, newBodyValue);
            }
            
            if (isTitleContentChanged)
            {
                SetContentTranslationValue(existingEntity, ContentTypes.TITLE, lang, newTitleValue);
                SetBaseLanguage(existingEntity, ContentTypes.TITLE, newBodyValue);
            }
        }
    }

    public static string GetContentValue(IContentEntity entity, string contentType, string languageCulture)
    {
        var result = entity.Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }

    public static void SetContentTranslationValue(IContentEntity entity, string contentType, string languageCulture,
        string value)
    {
        var result = entity.Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First();
        
        result.TranslationValue = value;
    }
    
    public static void SetBaseLanguage(IContentEntity entity, string contentType, string value)
    {
        var result = entity.Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString;
        result.Value = value;
    }
    
    /*
     * public string GetContentValue(string contentType, string languageCulture)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        return result;
    }

    public void SetContentTranslationValue(string contentType, string languageCulture, string value)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First();
        
        result.TranslationValue = value;
    }

    public void SetBaseLanguage(string contentType, string value)
    {
        var result = Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString;
        result.Value = value;
    }
     */
}

