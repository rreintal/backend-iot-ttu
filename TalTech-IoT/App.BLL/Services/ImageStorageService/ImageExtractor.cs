using System.Text.RegularExpressions;
using App.BLL.Contracts.ImageStorageModels.Save;

namespace App.BLL.Services.ImageStorageService;

public class ImageExtractor
{
    private const string PATTERN_IMAGE_NAME_FROM_LINK = @"[^\/]+\.(\w+)$";
    private const string PATTERN_BASE64 = @"^data:image\/(png|jpeg|jpg|webp);base64,([\/+A-Za-z0-9]+={0,2})$";
    
    public ImageExtractorResult? CreatePayloadFromBase64(string base64)
    {
        Regex regex = new Regex(PATTERN_BASE64);
        
        Match match = regex.Match(base64);
        
        if (match.Success)
        {
            string format = match.Groups[1].Value;
            string base64String = match.Groups[2].Value;

            return new ImageExtractorResult()
            {
                FileFormat = format,
                ImageContent = base64String
            };

        }
        return null;
    }

    public bool IsBase64String(string data)
    {
        Regex regex = new Regex(PATTERN_BASE64);
        return regex.IsMatch(data);
    }

    public string GetImageNameFromLink(string link)
    {
        Regex regex = new Regex(PATTERN_IMAGE_NAME_FROM_LINK);
        Match match = regex.Match(link);
        if (match.Success)
        {
            return match.Groups[0].Value;
        }

        throw new Exception($"ImageExtractor: GetImageNameFromLink({link}) did not find an image name!");  // TODO: what to do here?? 
    }
}