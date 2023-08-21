namespace App.BLL.Contracts;

public interface IThumbnailService
{
    public string Compress(string base64ImageData);
}