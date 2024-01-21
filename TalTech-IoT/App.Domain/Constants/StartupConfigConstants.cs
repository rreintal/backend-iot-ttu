namespace App.Domain;

public static class StartupConfigConstants
{
    public const string POPULATE_EXERCISE_TYPES = "DataInit:PopulateTypes";
    public const string DROP_DATABASE = "DataInit:DropDatabase";
    public const string MIGRATE_DATABASE = "DataInit:MigrateDatabase";
    
    public const string JWT_ISSUER = "JWT:Issuer";
    public const string JWT_AUDIENCE = "JWT:Audience";
    public const string JWT_KEY = "JWT:Key";
    public const string JWT_EXPIRATION_TIME = "JWT:ExpiresInSeconds";
}