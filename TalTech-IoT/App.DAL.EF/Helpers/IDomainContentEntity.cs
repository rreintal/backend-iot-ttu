namespace App.DAL.EF.Helpers;

public interface IDomainContentEntity
{
    public List<App.Domain.Content> Content { get; set; }

    public string GetContentValue(string contentType, string languageCulture);
}