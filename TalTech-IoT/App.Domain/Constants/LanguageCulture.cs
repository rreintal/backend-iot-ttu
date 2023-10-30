namespace App.Domain;

public class LanguageCulture
{
    // Defualt language
    public const string BASE_LANGUAGE = EST;
    
    public const string EST = "et";
    public const string ENG = "en";

    public static List<string> ALL_LANGUAGES = new List<string>() { EST, ENG };

}