namespace App.Domain;

public class LanguageCulture
{
    // Defualt language
    public static string BASE_LANGUAGE = EST;
    
    public static string EST = "et";
    public static string ENG = "en";

    public static List<string> ALL_LANGUAGES = new List<string>() { EST, ENG };

}