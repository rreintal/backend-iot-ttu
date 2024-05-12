using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Services.ImageStorageService.Models.Delete;

namespace App.BLL.Services.ImageStorageService;

public interface IIMageStorageExecutor
{
    public List<CDNSaveResult> Upload(CDNSaveImages payload, bool test = false);
}
public class ImageStorageExecutor : IIMageStorageExecutor
{
    private string IMAGES_DIRECTORY { get; set; }

    public ImageStorageExecutor()
    {
        var imagesDirectory = Environment.GetEnvironmentVariable("IMAGES_DIRECTORY");
        if (string.IsNullOrEmpty(imagesDirectory))
        {
            throw new Exception("ImageStorageExecutor: Environment variable: IMAGES_DIRECTORY - is not set or is empty!");
        }
        IMAGES_DIRECTORY = imagesDirectory;
    }
    public List<CDNSaveResult> Upload(CDNSaveImages payload, bool test)
    {
        var result = new List<CDNSaveResult>();
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
                string path = $"{IMAGES_DIRECTORY}{imageName}.{item.FileFormat}";
                byte[] imageByteArray = Convert.FromBase64String(item.ImageContent);

                // If its for test, then don't save it to the file system
                if (!test)
                {
                    File.WriteAllBytes(path, imageByteArray);   
                }

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
    
    
    public bool DeleteImages(List<CDNDeleteImage> imagesList, bool test = false)
    {
        Console.WriteLine("ProcessDelete hit!");
        var deletedImagesCount = 0;
        
        // If its a test, then there is no images to delete, and it can't be tested. so return true!!
        if (test)
        {
            return true;
        }
        
        foreach (var imageToDelete in imagesList)
        {
            var imageName = imageToDelete.ImageName;
            string filePath = $"{IMAGES_DIRECTORY}{imageName}";

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
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