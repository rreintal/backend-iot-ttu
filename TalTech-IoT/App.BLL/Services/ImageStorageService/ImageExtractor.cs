using System.Text.RegularExpressions;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Services.ImageStorageService.Models;
using App.BLL.Services.ImageStorageService.Models.Delete;
using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Services.ImageStorageService;

public class ImageExtractor
{
    private const string PATTERN_IMAGE_WITH_RESOURCE = @"<img\s+src=""[^""]+/([^""]+)""";
    private const string PATTERN_BASE64IMAGE = "<img src=\"data:image/(jpeg|png|jpg|webp);base64,([^\"]+)\"";
    
    /*
    public DeletePayload? ExtractImagesInCloud(string content)
    {
        var result = new DeletePayload()
        {
            Data = new List<Models.Delete.DeleteImage>()
        };
        
        
        // Regular expression pattern to match src attribute in img tags
        string pattern = PATTERN_IMAGE_WITH_RESOURCE;

        MatchCollection matches = Regex.Matches(content, pattern);
        // Iterate over the matches and extract image filenames
        foreach (Match match in matches)
        {
            // Extract image filename
            string filename = match.Groups[1].Value;
            result.Data.Add(
                new Models.Delete.DeleteImage()
                {
                    ImageName = filename
                });
        }

        // If no images were found!
        if (result.Data.IsNullOrEmpty())
        {
            return null;
        }

        return result;
    }
    */
    // See tagastab { seq, [{img, format, seq}] }

    public List<string>? ExtractSrcElementList(string content)
    {
        var srcElementRegex = "<img[^>]*>";
        List<string> srcTagsList = new List<string> {};

        MatchCollection matches = Regex.Matches(content, srcElementRegex);
        
        
        foreach (Match match in matches)
        {
            if (match.Success)
            {
                srcTagsList.Add(match.Value);
            }
        }

        if (srcTagsList.IsNullOrEmpty())
        {
            return null;
        }

        return srcTagsList;
    }
    public List<SaveImage>? ExtractBase64Images(string content)
    {
        var srcElementRegex = "<img[^>]*>";
        List<string> srcTagsList = new List<string> {};

        MatchCollection matches = Regex.Matches(content, srcElementRegex);
        
        
        foreach (Match match in matches)
        {
            if (match.Success)
            {
                srcTagsList.Add(match.Value);
            }
        }

        // If no images in content
        if (srcTagsList.IsNullOrEmpty())
        {
            // return original content
            return null;
        }

        var imageServicePayload = new List<SaveImage>() {};
        
        string base64Regex = PATTERN_BASE64IMAGE;

        foreach (var tag in srcTagsList)
        {
            Match match = Regex.Match(tag, base64Regex);
            if (match.Success)
            {
                var dto = new SaveImage()
                {
                    FileFormat = match.Groups[1].Value,
                    ImageContent = match.Groups[2].Value, 
                    Sequence = srcTagsList.IndexOf(tag) // TODO: if there are multiple same images, make some buffer list
                };
                imageServicePayload.Add(dto);
            }
        }

        return imageServicePayload;
    }
    
}