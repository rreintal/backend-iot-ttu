using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using App.BLL.Contracts;
using App.Domain.Constants;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace App.BLL.Services;

public class ImageStorageService : IImageStorageService
{
    
    private static readonly HttpClient _httpClient = new HttpClient();
    private const string PATTERN_IMAGE_WITH_RESOURCE = @"<img\s+src=""[^""]+/([^""]+)""";
    private const string PATTERN_BASE64IMAGE = "<img src=\"data:image/(jpeg|png|jpg|webp);base64,([^\"]+)\""; // TODO: init this in constructor and List.join(|) for file types!
    public ImageStorageService()
    {
        
    }

    private SaveImagesDTO? ExtractBase64Images(string content)
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

        var imageServicePayload = new SaveImagesDTO()
        {
            data = new List<SaveImageDTO>()
        };
        
        string base64Regex = PATTERN_BASE64IMAGE;

        foreach (var tag in srcTagsList)
        {
            Match match = Regex.Match(tag, base64Regex);
            if (match.Success)
            {
                var dto = new SaveImageDTO()
                {
                    FileFormat = match.Groups[1].Value,
                    ImageContent = match.Groups[2].Value, 
                    Sequence = srcTagsList.IndexOf(tag) // TODO: if there are multiple same images, make some buffer list
                };
                imageServicePayload.data.Add(dto);
            }
        }

        return imageServicePayload;
    }
    public async Task<string> ReplaceImages(string content)
    {
        // replace base string (languageString.value)
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
            return content;
        }
        
        var imageServicePayload = new SaveImagesDTO()
        {
            data = new List<SaveImageDTO>()
        };
        
        string base64Regex = PATTERN_BASE64IMAGE;

        foreach (var tag in srcTagsList)
        {
            Match match = Regex.Match(tag, base64Regex);
            if (match.Success)
            {
                var dto = new SaveImageDTO()
                {
                    FileFormat = match.Groups[1].Value,
                    ImageContent = match.Groups[2].Value, 
                    Sequence = srcTagsList.IndexOf(tag) // TODO: if there are multiple same images, make some buffer list
                };
                imageServicePayload.data.Add(dto);
            }
        }
        
        var saveImagesResultsDto = await Save(imageServicePayload);

        foreach (var resultDto in saveImagesResultsDto!.Results)
        {
            var baseLoc = ImageStorageServiceConstants.IMAGE_PUBLIC_LOCATION;
            var itemFromOriginalArray = srcTagsList[resultDto.Sequence];
            var newItem = $"<img src=\"{baseLoc}{resultDto.Link}\">"; // TODO: remove this .png here
            content = content.Replace(itemFromOriginalArray, newItem);
        }
        
        return content;
    }
    
    // PROLLY EI TOHI OLLA ASYNC?
    private async Task<SaveImagesResultsDTO?> Save(SaveImagesDTO payload)
    {
        try
            {
                var response = await _httpClient.PostAsJsonAsync(ImageStorageServiceConstants.UPLOAD_IMAGE, payload);
                var responseData = await response.Content.ReadFromJsonAsync<SaveImagesResultsDTO>();
                return responseData!;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        return null;
    }

    public bool Delete(string content)
    {
        var payload = new List<DeleteImage>() { };
        
        // Regular expression pattern to match src attribute in img tags
        string pattern = PATTERN_IMAGE_WITH_RESOURCE;

        MatchCollection matches = Regex.Matches(content, pattern);

        // Iterate over the matches and extract image filenames
        foreach (Match match in matches)
        {
            // Extract image filename
            string filename = match.Groups[1].Value;
            payload.Add(new DeleteImage() {ImageName = filename});
            Console.WriteLine($"Filename: {filename}");
        }

        // If no images were found!
        if (payload.IsNullOrEmpty())
        {
            return true;
        }
        
        try
        {
            var deletionSuccess = ExecuteDelete(payload);
            return deletionSuccess;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught when deleting images!");
            Console.WriteLine("Message :{0} ", e.Message);
        }

        return false;
    }

    public async Task<string> Update(string oldContent, string newContent)
    {
        // Get all old content images
        var oldContentImages = new List<string>() { };
        var imageInCdnPattern = PATTERN_IMAGE_WITH_RESOURCE;
        var oldContentMatches = Regex.Matches(oldContent, imageInCdnPattern);
        
        // Get all new content images (img in cdn)

        foreach (Match match in oldContentMatches)
        {
            string filename = match.Groups[1].Value;
            oldContentImages.Add(filename);
        }
        
        // Get all the new content images (img in cdn)

        var newContentImages = new List<string>() { };
        var newContentMatches = Regex.Matches(newContent, imageInCdnPattern);
        foreach (Match match in newContentMatches)
        {
            var fileName = match.Groups[1].Value;
            newContentImages.Add(fileName);
        }

        var imagesToDelete = new List<DeleteImage>() { };
        
        // Find items in oldContentList that are not present in newContentList
        var itemsRemoved = oldContentImages.Except(newContentImages).ToList();

        if (itemsRemoved.Any())
        {
            foreach (var item in itemsRemoved)
            {
                imagesToDelete.Add(new DeleteImage() { ImageName = item});
            }
        }
        // If there are images to delete!
        if (!imagesToDelete.IsNullOrEmpty())
        {
            try
            {
                // Delete (OLD_IMAGES)
                var deletionSuccess = ExecuteDelete(imagesToDelete);
                
                // TODO: what to do here if it fails???
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught when deleting images!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            
        }

        // Get all the new images which are not in the content (NEW_IMAGES)
        //var newImagesPayload = ExtractBase64Images(newContent);

        // TODO: refactor this replace images shit!
        await ReplaceImages(newContent);

        // Post all the new images

        // return modified content
        return newContent;
    }
    
    private bool ExecuteDelete(List<DeleteImage> imagesList)
    {
        try
        {
            var postPayload = new StringContent(JsonConvert.SerializeObject(imagesList), Encoding.UTF8, "application/json");
            var response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Delete, ImageStorageServiceConstants.DELETE_IMAGE)
            {
                Content = postPayload
            });

            Console.WriteLine($"RESPONSE CODE: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Images deleted successfully!");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                bool success = bool.Parse(responseContent);
                return success;
            }
            else
            {
                Console.WriteLine("Failed to delete images!");
                return false;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught when deleting images!");
            Console.WriteLine("Message :{0} ", e.Message);
            return false;
        }
    }
}