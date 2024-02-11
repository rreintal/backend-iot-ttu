using System.Net.Http.Json;
using App.BLL.Contracts;
using App.Domain.Constants;
using BLL.DTO.ContentHelper;
using BLL.DTO.V1;
using Public.DTO.Content;

namespace App.BLL.Services;

public class ImageStorageService : IImageStorageService
{
    public async Task<SaveImagesResultsDTO?> Save(SaveImagesDTO payload)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.PostAsJsonAsync(ImageStorageServiceConstants.UPLOAD_IMAGE, payload);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadFromJsonAsync<SaveImagesResultsDTO>();
                return responseData!;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        return null;
    }

    /*
    private async Task<string> HandleImageContent(string content)
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

        var imageServicePayload = new SaveImagesDTO()
        {
            data = new List<SaveImageDTO>()
        };
        
        string base64Regex = "<img src=\"data:image/(jpeg|png|jpg|webp);base64,([^\"]+)\"";

        foreach (var tag in srcTagsList)
        {
            Match match = Regex.Match(tag, base64Regex);
            if (match.Success)
            {
                var dto = new SaveImageDTO()
                {
                    // march.Groups[1] for the file format (jpeg/png ... )
                    // match.Groups[2].Value to get the base64 data
                    FileFormat = match.Groups[1].Value,
                    ImageContent = match.Groups[2].Value, 
                    Sequence = srcTagsList.IndexOf(tag) // TODO: if there are multiple same images, make some buffer list
                };
                imageServicePayload.data.Add(dto);
            }
        }
        
        var saveImagesResultsDto = await _imageStorageService.Save(imageServicePayload);

        foreach (var resultDto in saveImagesResultsDto!.Results)
        {
            var baseLoc = ImageStorageServiceConstants.IMAGE_PUBLIC_LOCATION;
            var itemFromOriginalArray = srcTagsList[resultDto.Sequence];
            var newItem = $"<img src=\"{baseLoc}{resultDto.Link}.png\">"; // TODO: remove this .png here
            content = content.Replace(itemFromOriginalArray, newItem);
        }
        return content;
    }
    */

    public void Delete(DeleteImageDTO payload)
    {
        throw new NotImplementedException();
    }
}