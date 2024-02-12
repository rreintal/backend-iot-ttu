
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Services.ImageStorageService.Models;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.Domain.Constants;
using BLL.DTO.V1;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.BLL.Services.ImageStorageService;

 
public class ImageStorageService : IImageStorageService
{
    
    private static readonly HttpClient _httpClient = new HttpClient();
    private ImageExtractor _imageExtractor { get; }

    public ImageStorageService()
    {
        _imageExtractor = new ImageExtractor();
    }


    public async Task<List<SaveResult>> Save(SaveContent data)
    {
        Dictionary<int, string> bufferMap = new Dictionary<int, string>(); // (sequence, OriginalContent)
        Dictionary<int, List<SaveImage>?> payloadDict = new Dictionary<int, List<SaveImage>?>(); // (sequence, OriginalContentImagesToSave)
        Dictionary<int, List<string>?> srcTagDict = new Dictionary<int, List<string>?>(); // siin on (sequence, OriginalContentSrcTagList [replace jaoks]) 

        foreach (var originalContent in data.Items)
        {
            bufferMap[originalContent.Sequence] = originalContent.Content;
        }

        // Get every content images which need to be saved
        foreach (var itemToSave in data.Items)
        {
            var imagesToSave = _imageExtractor.ExtractBase64Images(itemToSave.Content);
            if (imagesToSave.IsNullOrEmpty())
            {
                // If content does not have any images to save
            }
            else
            {
                var srcTagList = _imageExtractor.ExtractSrcElementList(itemToSave.Content);
                payloadDict[itemToSave.Sequence] = imagesToSave;
                srcTagDict[itemToSave.Sequence] = srcTagList;   
            }
        }

        var CDNPayload = new CDNSaveImages()
        {
            Items = new List<CDNSaveUnit>()
        };

        foreach (var pair in payloadDict)
        {
            var sequenceNumber = pair.Key;
            
            var items = pair.Value;
            var unit = new CDNSaveUnit()
            {
                Sequence = sequenceNumber,
                Items = items
            };
            CDNPayload.Items.Add(unit);

        }
        
        // Send images to save

        var response = await _httpClient.PostAsJsonAsync(ImageStorageServiceConstants.UPLOAD_IMAGE, CDNPayload);
        var responseJson = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var responseData = JsonSerializer.Deserialize<List<CDNSaveResult>>(responseJson, options);
        // Replace all images from original string with the updated link!

        var result = new List<SaveResult>() { };
        // võta originaal content
        foreach (var originalData in data.Items)
        {

            // Check if content with this sequence even needs updating!
            if (srcTagDict.ContainsKey(originalData.Sequence))
            {
                // võta originaalContenti srcTagList
                var originalContentSrcTagList = srcTagDict[originalData.Sequence];
                
                // võta response item
                var responseListWithContentLinks = responseData.First(e => e.Sequence == originalData.Sequence);
            
                // replace originaalContentis srcTag uue srcTagiga
                var updatedContent = "";
            
                foreach (var newLink in responseListWithContentLinks.Items)
                {
                    var newItem = $"<img src=\"{ImageStorageServiceConstants.IMAGE_PUBLIC_LOCATION}{newLink.Link}\">"; // Link = Image name on server 'abc.png'
                    var oldItem = originalContentSrcTagList![newLink.Sequence];
                    updatedContent = originalData.Content.Replace(oldItem, newItem);
                }

                var resultItem = new SaveResult()
                {
                    Sequence = originalData.Sequence,
                    UpdatedContent = updatedContent
                };
                result.Add(resultItem);
            }


        }
        return result;
    }
    
    public bool Delete(string content)
    {
        throw new NotImplementedException();
    }

    public string ReplaceImages(string content)
    {
        /*
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
        
        var imageServicePayload = new SavePayload()
        {
            Data = new List<SaveImage>()
        };
        
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
                imageServicePayload.data.Add(dto);
            }
        }
        
        var saveImagesResultsDto = Save(imageServicePayload);

        foreach (var resultDto in saveImagesResultsDto!.Results)
        {
            var baseLoc = ImageStorageServiceConstants.IMAGE_PUBLIC_LOCATION;
            var itemFromOriginalArray = srcTagsList[resultDto.Sequence];
            var newItem = $"<img src=\"{baseLoc}{resultDto.Link}\">"; // TODO: remove this .png here
            content = content.Replace(itemFromOriginalArray, newItem);
        }
        */
        throw new NotImplementedException();
        return content;
    }

    //private SaveImagesResultsDTO? Save(Content content)
    //{
        /*
        try
        {
            var postPayload = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Post, ImageStorageServiceConstants.UPLOAD_IMAGE)
            {
                Content = postPayload
            });
                
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<SaveImagesResultsDTO>(responseContent);
                return result;
            }
            else
            {
                // TODO: what happens if it fails?
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        return null;
        */
    //}

    /*
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

    public string Update(string oldContent, string newContent)
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
        ReplaceImages(newContent);

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
    */
}