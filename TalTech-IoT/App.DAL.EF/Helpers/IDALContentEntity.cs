namespace App.DAL.EF.Helpers;

public interface IDALContentEntity
{
    public string GetContentValue(string contentType, string languageCulture);
}