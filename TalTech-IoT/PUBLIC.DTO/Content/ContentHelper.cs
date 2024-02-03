using App.Domain;
using Base.Domain;
using BLL.DTO.ContentHelper;
using Microsoft.Extensions.Logging;
using Public.DTO.V1;
using ContentType = BLL.DTO.V1.ContentType;
using News = Public.DTO.V1.News;

namespace Public.DTO.Content;

public abstract class ContentHelper
{
    public enum EContentHelperEntityType
    {
        News,
        Project,
        PageContent
    }
    // tee constid, millega saad teada kas on News, Proejct blabla

    /// <summary>
    /// Helper to create content based on type (Title, Body)
    /// </summary>
    /// <param name="contentDtoList"></param>
    /// <param name="contentType"></param>
    /// <param name="entityId"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static BLL.DTO.V1.Content CreateContent(List<ContentDto> contentDtoList, ContentType contentType, Guid entityId, EContentHelperEntityType entityType)
    {
        var newsId = entityType == EContentHelperEntityType.News ? entityId : (Guid?)null;
        var projectId = entityType == EContentHelperEntityType.Project ? entityId : (Guid?)null;
        var pageContentId = entityType == EContentHelperEntityType.PageContent ? entityId : (Guid?)null;
        
        var baseContentDto = contentDtoList.First(x => x.Culture == LanguageCulture.BASE_LANGUAGE);
        var languageStringId = Guid.NewGuid();
        var languageString = new BLL.DTO.V1.LanguageString()
        {
            Id = languageStringId,
            Value = baseContentDto.Value
        };
        var content = new BLL.DTO.V1.Content()
        {
            ContentTypeId = contentType.Id,
            LanguageStringId = languageStringId,
            LanguageString = languageString,
            NewsId = newsId,
            ProjectId = projectId,
            PageContentId = pageContentId,
            ContentType = contentType
        };

        languageString.Content = content;

        var translations = new List<BLL.DTO.V1.LanguageStringTranslation>();
        foreach (var contentDto in contentDtoList)
        {
            var langStr = new BLL.DTO.V1.LanguageStringTranslation()
            {
                LanguageCulture = contentDto.Culture,
                TranslationValue = contentDto.Value,
                LanguageStringId = languageString.Id
            };
            translations.Add(langStr);
        }

        languageString.LanguageStringTranslations = translations;
        var result = new BLL.DTO.V1.Content()
        {
            ContentTypeId = contentType.Id,
            LanguageStringId = languageString.Id,
            LanguageString = languageString,
            NewsId = newsId,
            ProjectId = projectId,
            PageContentId = pageContentId,
            ContentType = contentType
        };

        return result;
    }

    public static string? GetContentValue(IContentEntity entity, string contentType, string languageCulture)
    {
        var result = entity.Content
            .First(content => content.ContentType?.Name == contentType)
            .LanguageString?.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        if (result == null)
        {
            Console.WriteLine($"ContentHelper: for entity with type {entity.GetType()} could not get content value.");
        }
        return result;
    }
}