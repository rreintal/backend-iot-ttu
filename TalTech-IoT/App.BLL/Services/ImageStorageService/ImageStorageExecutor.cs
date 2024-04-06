using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Services.ImageStorageService.Models.Delete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Services.ImageStorageService;

public interface IIMageStorageExecutor
{
    public List<CDNSaveResult> Upload(CDNSaveImages payload);
    public bool DeleteImages(List<CDNDeleteImage> imagesList);
}

// TODO: make it atomic, that first conver everything from base64 to byteArray, and after everything successful, then write.

public class ImageStorageExecutor : IIMageStorageExecutor
{
    private string IMAGES_DIRECTORY = "/Users/richardreintal/dev/images/"; // use env variable from constructor, if not set then throw fatal error

    public ImageStorageExecutor()
    {
        var imagesDirectory = Environment.GetEnvironmentVariable("IMAGES_DIRECTORY");
        var isValueMissing = imagesDirectory.IsNullOrEmpty();
        if (isValueMissing)
        {
            throw new Exception("ImageStorageExecutor: Environemnt variable: IMAGES_DIRECTORY - is not set or is empty!");
        }
        IMAGES_DIRECTORY = imagesDirectory;
    }
    public List<CDNSaveResult> Upload(CDNSaveImages payload)
    {
        var result = new List<CDNSaveResult>() { };
        foreach (var unit in payload.Items)
        {
            var saveItem = new CDNSaveResult()
            {
                Sequence = unit.Sequence,
                Items = new List<CDNSaveResultItem>()
            };
            foreach (var item in unit.Items)
            {
                // Save image
                string imageName = Guid.NewGuid().ToString();
                string path = $"{IMAGES_DIRECTORY}{imageName}.{item.FileFormat}"; // TTÜ
                byte[] imageByteArray = Convert.FromBase64String(item.ImageContent);
                File.WriteAllBytes(path, imageByteArray);

                var saveResultItem = new CDNSaveResultItem()
                {
                    Sequence = item.Sequence,
                    Link = $"{imageName}.{item.FileFormat}"
                };
                saveItem.Items.Add(saveResultItem);

            }

            result.Add(saveItem);
        }

        foreach (var saveresultItem in result)
        {
            Console.WriteLine($"This is unit sequence: {saveresultItem.Sequence}");
            foreach (var i in saveresultItem.Items)
            {
                Console.WriteLine($"Sequence in unit: {i.Sequence}, item {i.Link}");
            }
        }

        return result;
    }
    
    
    public bool DeleteImages(List<CDNDeleteImage> imagesList)
    {
        Console.WriteLine("Delete hit!");
        var deletedImagesCount = 0;
        foreach (var imageToDelete in imagesList)
        {
            var imageName = imageToDelete.ImageName;
            string filePath = $"{IMAGES_DIRECTORY}{imageName}";

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    deletedImagesCount++;
                    Console.WriteLine($"Deleted image: {imageName}");
                }
                else
                {
                    Console.WriteLine($"File '{imageName}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }  
        }

        
        return imagesList.Count == deletedImagesCount;
    }
}