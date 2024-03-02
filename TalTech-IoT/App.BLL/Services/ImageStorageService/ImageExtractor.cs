using System.Security;
using System.Text.RegularExpressions;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Services.ImageStorageService.Models;
using App.BLL.Services.ImageStorageService.Models.Delete;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Services.ImageStorageService;

public class ImageExtractor
{
    /// <summary>
    /// This detects <img src="http://172.16.0.87/images/bc9dc544-0ab2-48c3-8dee-e8cdafaaa09f.png">  the image name 'bc9dc544-0ab2-48c3-8dee-e8cdafaaa09f.png'
    /// </summary>
    private const string PATTERN_IMAGE_WITH_RESOURCE = @"(?<=\/images\/)[^""]+";
    
    /// <summary>
    /// This detects  <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==">
    /// </summary>
    private const string PATTERN_BASE64IMAGE = "<img src=\"data:image/(jpeg|png|jpg|webp);base64,([^\"]+)\">";

    private const string PATTERN_IMAGE_NAME_FROM_LINK = @"[^\/]+\.(\w+)$";
    
    

    // ----------------- Katse 2
    private const string PATTERN_BASE64 = @"^data:image\/(png|jpeg|jpg|webp);base64,([\/+A-Za-z0-9]+={0,2})$";


    // TODO: nullable, but will never return null?
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
        // TODO: add logger here that it failed
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
    
    
    
    // ----------------- Katse 2
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public List<string>? ExtractBase64ImageSrcTag(string content)
    {
        //var srcElementRegex = "<img[^>]*>";
        var srcElementRegex = PATTERN_BASE64IMAGE;
        List<string> srcTagsList = new List<string> {};

        RegexHelper(srcElementRegex, content, (match => srcTagsList.Add(match)));
        if (srcTagsList.IsNullOrEmpty())
        {
            return null;
        }

        return srcTagsList;
    }
    public List<SaveImage>? ExtractBase64ImagesWithFormat(string content)
    {
        List<string> srcTagsList = new List<string> {};
        var imageServicePayload = new List<SaveImage>() {};
        
        var srcElementRegex = PATTERN_BASE64IMAGE;
        

        RegexHelper(srcElementRegex, content, (match => srcTagsList.Add(match)));

        // If no images in content
        if (srcTagsList.IsNullOrEmpty())
        {
            // return original content
            return null;
        }

            
        
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

    /// <summary>
    /// Gets all the image names (aaaa-aaaa-aaaa-aaaa.png) which are already saved on server
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public List<string> ExtractImageWithResource(string content)
    {
        var result = new List<string>();
        RegexHelper(PATTERN_IMAGE_WITH_RESOURCE, content, match => result.Add(match));
        return result;
    }

    /// <summary>
    /// Returns the list of items (<src href=image source inside server">) which are not present in the new content
    /// and are so called 'unused'
    /// </summary>
    /// <param name="oldContent"></param>
    /// <param name="newContent"></param>
    /// <returns></returns>
    public List<string>? ExtractUnusedImages(string oldContent, string newContent)
    {
        // TODO: this can also return null!
        List<string> oldContentSrcTagWithImageURL = new List<string>();
        List<string> newContentSrcTagWithImageURL = new List<string>();
        RegexHelper(PATTERN_IMAGE_WITH_RESOURCE, oldContent, match => oldContentSrcTagWithImageURL.Add(match));
        RegexHelper(PATTERN_IMAGE_WITH_RESOURCE, newContent, match => newContentSrcTagWithImageURL.Add(match));
        var unusedImages = oldContentSrcTagWithImageURL.Except(newContentSrcTagWithImageURL).ToList();
        return unusedImages.IsNullOrEmpty() ? null : unusedImages;
    }
    
    private void RegexHelper(string pattern, string content, Action<string> callBack)
    {
        // Use Regex to find matches of 'pattern' in 'content'
        var matches = Regex.Matches(content, pattern);

        // For each match, invoke the callback with the match's value
        foreach (Match match in matches)
        {
            callBack(match.Value);
        }
    }
    
}