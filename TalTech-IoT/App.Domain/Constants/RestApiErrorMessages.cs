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

    /// <summary>
    /// Mail service
    /// </summary>
    public const string MissingMailRecipent = "MISSING_MAIL_RECIPENT";

    /// <summary>
    /// Projects
    /// </summary>
    public const string MissingProjectManager = "MISING_PROJECT_MANAGER"; // TODO: kas vajalik?
    public const string MissingProjectVolume = "MISSING_PROJECT_VOLUME"; // TODO: kas vajalik?
    public const string MissingProjectYear = "MISSING_PROJECT_YEAR";


    /// <summary>
    /// Users
    /// </summary>
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string UserUsernameAlreadyExists = "USERNAME_ALREADY_EXISTS";
    public const string UserEmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
    public const string UserAlreadyUnlocked = "USER_ALREADY_UNLOCKED";
    public const string UserAlreadyLocked = "USER_ALREADY_LOCKED";
    public const string UserGeneralError = "USERNAME_PASSWORD_PROBLEM"; // this is general, as if password/email is wrong. not saying which because of security

    /// <summary>
    /// Roles
    /// </summary>
    public const string RoleNotFound = "ROLE_NOT_FOUND";
}

/*

General
NOT_FOUND
MISSING_TRANSLATION_VALUE -> kasutaja annab 1 contenti koos translationiga
INVALID_TRANSLATION_LANGUAGE_STRING -> kui kasutaja annab vale lang str

Middleware
INVALID_LANGUAGE_STRING @Tee middleware, mis juba enne detectib kas lang str on õige

TopicArea
TOPIC_AREA_CREATE_PARENT_DOES_NOT_EXIST
TOPIC_AREA_CREATE_NAME_EXISTS
general



Mail - get repo
SERIVCE_UNAVAILABLE (503)

Mail - contact us
MESSAGE_TOO_LONG -> validation 

News
MISSING_AUTHOR 
TOO_BIG_IMAGE -> validation
IMAGE_MISSING 
TOO_MANY_TOPIC_AREAS -> validation
general 


PROJECT
INVALID_YEAR -> validation (positive number, maxValue < 3000)
general

TODO: Küsi kliendi käest
MISING_PROJECT_MANAGER -> validation
MISSING_PROJECT_VOLUME -> validation
*/