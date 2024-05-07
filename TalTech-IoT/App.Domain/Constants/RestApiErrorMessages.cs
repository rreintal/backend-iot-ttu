namespace App.Domain.Constants;

public static class RestApiErrorMessages
{
    /// <summary>
    ///  General
    /// </summary>
    public const string GeneralNotFound = "NOT_FOUND";

    public const string AlreadyExists = "ALREADY_EXISTS";
    public const string GeneralMissingTranslationValue = "MISSING_TRANSLATION_VALUE"; // If translation value is missing
    public const string GeneralInvalidLanguageCulture = "INVALID_TRANSLATION_LANGUAGE_CULTURE"; // if contains invalid language culture
    public const string GeneralMissingAuthor = "MISSING_AUTHOR";
    public const string GeneralMissingTopicArea = "MISISNG_TOPIC_AREA";
    public const string InvalidEmail = "INVALID_EMAIL";

    /// <summary>
    /// Mail service
    /// </summary>
    public const string MissingMailRecipent = "MISSING_MAIL_RECIPENT";

    /// <summary>
    /// TopicArea
    /// </summary>
    public const string TopicAreaHasAssociatedNews = "TOPICAREA_HAS_NEWS";

    /// <summary>
    /// Projects
    /// </summary>
    public const string MissingProjectManager = "MISING_PROJECT_MANAGER"; // TODO: kas vajalik?
    public const string MissingProjectVolume = "MISSING_PROJECT_VOLUME"; // TODO: kas vajalik?
    public const string MissingProjectYear = "MISSING_PROJECT_YEAR";
    public const string InvalidProjectYear = "PROJECT_YEAR_MUST_BE_1000_TO_3000";

    /// <summary>
    /// FeedPageCategory
    /// </summary>
    public const string FeedPageCategoryHasPosts = "CATEGORY_HAS_POSTS";


    /// <summary>
    /// Users
    /// </summary>
    public const string UserUsernameAlreadyExists = "USERNAME_ALREADY_EXISTS";
    public const string UserEmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
    public const string UserGeneralError = "USERNAME_PASSWORD_PROBLEM"; // this is general, as if password/email is wrong. not saying which because of security
    public const string UserDeleteHimselfError = "USER_CANT_DELETE_ITSELF";

    /// <summary>
    /// Roles
    /// </summary>
    public const string RoleNotFound = "ROLE_NOT_FOUND";
}