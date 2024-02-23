using App.Domain;
using BLL.DTO.ContentHelper;
using Public.DTO.V1;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.Content;

public abstract class ContentHelper
{
    public enum EContentHelperEntityType
    {
        News,
        Project,
        PageContent,
        HomePageBanner,
        ContactPerson,
        FeedPageCategory,
        FeedPagePost,
        OpenSourceSolution
    }
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
        var homePageBannerId = entityType == EContentHelperEntityType.HomePageBanner ? entityId : (Guid?)null;
        var contactPersonId = entityType == EContentHelperEntityType.ContactPerson ? entityId : (Guid?)null;
        var feedPageCategoryId = entityType == EContentHelperEntityType.FeedPageCategory ? entityId : (Guid?)null;
        var feedPagePostId = entityType == EContentHelperEntityType.FeedPagePost ? entityId : (Guid?)null;
        var openSourceSolutionId = entityType == EContentHelperEntityType.OpenSourceSolution ? entityId : (Guid?)null;
        
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
            ContentType = contentType,
            HomePageBannerId = homePageBannerId,
            ContactPersonId = contactPersonId,
            FeedPageCategoryId = feedPageCategoryId,
            FeedPagePostId = feedPagePostId,
            OpenSourceSolutionId = openSourceSolutionId
            
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
            HomePageBannerId = homePageBannerId,
            ContentType = contentType,
            ContactPersonId = contactPersonId,
            FeedPageCategoryId = feedPageCategoryId,
            FeedPagePostId = feedPagePostId,
            OpenSourceSolutionId = openSourceSolutionId
        };

        return result;
    }
    
    public static void SetContentTranslationValue(IContentEntity entity, string contentType, string languageCulture, string value)
    {
        var result = entity.Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString!.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First();
        
        result.TranslationValue = value;
    }

    public static void SetBaseLanguage(IContentEntity entity,string contentType, string value)
    {
        var result = entity.Content.First(c => c.ContentType!.Name == contentType)
            .LanguageString;
        result.Value = value;
    }
    
    public static string? GetContentValue(IContentEntity entity, string contentType, string languageCulture)
    {
        var result = entity.Content
            .First(content => content.ContentType?.Name == contentType)
            .LanguageString?.LanguageStringTranslations
            .Where(translation => translation.LanguageCulture == languageCulture).First().TranslationValue;
        if (result == null)
        {
            Console.WriteLine($"ContentHelper (BLL): for entity with type {entity.GetType()} could not get content value.");
        }
        return result;
    }

    public static string? GetContentValue(BLL.DTO.V1.Content content, string languageCulture)
    {
        try
        {
            return content.LanguageString.LanguageStringTranslations.Where(t => t.LanguageCulture == languageCulture)
                .First()
                .TranslationValue;
        }
        catch
        {
            Console.WriteLine($"EXCEPTION CAUGHT IN CONTENTHELPER - could not read content with languageculture {languageCulture}");
            return null;
        }
        
    }

    public static string GetContentBaseValue(BLL.DTO.V1.Content content)
    {
        return content.LanguageString.Value;
    }

}