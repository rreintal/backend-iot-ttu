using System.Text.RegularExpressions;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Services.ImageStorageService.Models;
using App.BLL.Services.ImageStorageService.Models.Delete;
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
        var srcElementRegex = PATTERN_BASE64IMAGE;
        List<string> srcTagsList = new List<string> {};

        RegexHelper(srcElementRegex, content, (match => srcTagsList.Add(match)));

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