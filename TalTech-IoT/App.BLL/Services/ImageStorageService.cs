using System.Net.Http.Json;
using App.BLL.Contracts;
using App.Domain.Constants;

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

    public void Delete(DeleteImageDTO payload)
    {
        throw new NotImplementedException();
    }
}