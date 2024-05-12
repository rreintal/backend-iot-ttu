namespace WebApp;


/// <summary>
/// Class for holding environment variable constants
/// </summary>
public static class EnvironmentVariableConstants
{
    /// <summary>
    /// JWT ISSUER
    /// </summary>
    public const string JWT_ISSUER = "IOT_JWT_ISSUER";
    
    /// <summary>
    /// JWT AUDIENCE
    /// </summary>
    public const string JWT_AUDIENCE = "IOT_JWT_AUDIENCE";
    
    /// <summary>
    /// JWT_KEY
    /// </summary>
    public const string JWT_KEY = "JWT_KEY";
    
    /// <summary>
    /// DB Connection string.
    /// </summary>
    public const string DB_CONNECTION = "DB_CONNECTION_STRING";
    
    /// <summary>
    /// Drop database ("true" / "false")
    /// </summary>
    public const string DROP_DB = "DROP_DATABASE";
    /// <summary>
    /// Seed database ("true" / "false")
    /// </summary>
    public const string SEED_DB = "SEED_DATABASE";
    
    /// <summary>
    /// Migrate database ("true" / "false")
    /// </summary>
    public const string MIGRATE_DB = "MIGRATE_DATABASE";
    
    
    /// <summary>
    /// App admin email.
    /// </summary>
    public const string ADMIN_EMAIL = "APP_ADMIN_EMAIL";
    
    /// <summary>
    /// App admin password.
    /// </summary>
    public const string ADMIN_PASSWORD = "APP_ADMIN_PASSWORD";
}