using App.Domain.Contracts;

namespace App.Domain.Helpers;

public static class UpdateContentHelper
{
    public static void UpdateContent(IContentEntity existingEntity, IContentEntity newEntity, bool UpdateBody = true, bool UpdateTitle = true)
    {
        var cults = LanguageCulture.ALL_LANGUAGES;
        foreach (var lang in cults)
        {
            if (UpdateBody)
            {
                var oldBodyValue = GetContentValue(existingEntity, ContentTypes.BODY, lang);
                var newBodyValue = GetContentValue(newEntity, ContentTypes.BODY, lang);
                var isBodyValueChanged = oldBodyValue != newBodyValue;
                if (isBodyValueChanged)
                {
                    SetContentTranslationValue(existingEntity, ContentTypes.BODY, lang, newBodyValue);
                }   
            }

            if (UpdateTitle)
            {
                var newTitleValue = GetContentValue(newEntity, ContentTypes.TITLE, lang);
                var oldTitleValue = GetContentValue(existingEntity, ContentTypes.TITLE, lang);
                var isTitleContentChanged = oldTitleValue != newTitleValue;
                if (isTitleContentChanged)
                {
                    SetContentTranslationValue(existingEntity, ContentTypes.TITLE, lang, newTitleValue);
                }   
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
    
    
}

