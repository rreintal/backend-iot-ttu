
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Contracts.ImageStorageModels.Update;
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
        // TODO: check if it even needs to send any data, if not don't bother!
        
        Dictionary<int, List<SaveImage>?> payloadDict = new Dictionary<int, List<SaveImage>?>(); // (sequence, OriginalContentImagesToSave)
        Dictionary<int, List<string>?> srcTagDict = new Dictionary<int, List<string>?>(); // siin on (sequence, OriginalContentSrcTagList [replace jaoks]) 

        // Get every content images which need to be saved
        foreach (var itemToSave in data.Items)
        {
            // Thumbnailiga probleem siin, see REGEX ei catchi seda
            var imagesToSave = _imageExtractor.ExtractBase64ImagesWithFormat(itemToSave.Content);
            if (imagesToSave.IsNullOrEmpty())
            {
                // If content does not have any images to save
            }
            else
            {
                var srcTagList = _imageExtractor.ExtractBase64ImageSrcTag(itemToSave.Content);
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
                
                
                // this is because when it does not have any images to save, then it wont override the existing content with ""!!
                
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

    public async Task<List<UpdateResult>> Update(UpdateContent data)
    {
        var deleteData = new DeleteContent()
        {
            Items = new List<DeletePayloadContent>()
        };

        foreach (var itemToUpdate in data.Items)
        {
            var unusedImages = _imageExtractor.ExtractUnusedImages(itemToUpdate.OldContent, itemToUpdate.NewContent);
            if (unusedImages != null)
            {
                unusedImages.ForEach(item =>
                {
                    deleteData.Items.Add(new DeletePayloadContent()
                    {
                        Content = item
                    });
                });
            }
        }

        // If there are some unused images to delete!
        if (!deleteData.Items.IsNullOrEmpty())
        {
            var isDeleteSuccessful = await Delete(deleteData, true);
            if (!isDeleteSuccessful)
            {
                // TODO: what to do here if delete fails???
            }
        }
        
        // Update new iamges
        var saveData = new SaveContent()
        {
            Items = new List<SaveItem>()
        };
        Dictionary<int, string> newContentDict = new Dictionary<int, string>();

        foreach (var dataToUpdate in data.Items)
        {
            // lisa uus content
            newContentDict[dataToUpdate.Sequence] = dataToUpdate.NewContent;
        }

        foreach (var pair in newContentDict)
        {
            var saveItem = new SaveItem()
            {
                Content = pair.Value,
                Sequence = pair.Key
            };
            saveData.Items.Add(saveItem);
        }


        
        var result = new List<UpdateResult>() { };
        // TODO ADD CHECK HERE IF SAVEDATA == 0 THEN DONT EVEN SEND IT!
        var saveResult = await Save(saveData);
        // if there was something to save
        if (!saveResult.IsNullOrEmpty())
        {
            foreach (var saveItem in saveResult)
            {
                result.Add(new UpdateResult()
                {
                    NewContent = saveItem.UpdatedContent,
                    Sequence = saveItem.Sequence
                });   
            }

            if (saveResult.Count != data.Items.Count)
            {
                foreach (var item in data.Items)
                {
                    // If there is nothing to update, then still add the content as UpdateResult
                    var isItemUpdated = saveResult.FirstOrDefault(e => e.Sequence == item.Sequence) != null;
                    if (!isItemUpdated)
                    {
                        result.Add(new UpdateResult()
                        {
                            Sequence = item.Sequence,
                            NewContent = item.NewContent
                        });
                    }
                }
            }

            return result;    
        }
        else
        {
            foreach (var originalItems in data.Items)
            {
                result.Add(new UpdateResult()
                {
                    NewContent = originalItems.NewContent,
                    Sequence = originalItems.Sequence
                });
            }

            return result;
        }
        
    }


    /// <summary>
    /// This deletes images, if "alreadyImages = true" it means that the content already contains only image name for example "aaaa-aaaa-aaaa-aaaa.pdf"
    /// </summary>
    /// <param name="content"></param>
    /// <param name="alreadyImages"></param>
    /// <returns></returns>
    public async Task<bool> Delete(DeleteContent content, bool alreadyImages = false)
    {
        List<CDNDeleteImage> imagesToDelete = new List<CDNDeleteImage>();
        if (!alreadyImages)
        {
            foreach (var contentToDelete in content.Items)
            {
                var extractedLinks = _imageExtractor.ExtractImageWithResource(contentToDelete.Content);
                extractedLinks?.ForEach(e => imagesToDelete.Add(
                    new CDNDeleteImage()
                    {
                        ImageName = e
                    }));
            }    
        }
        else
        {
            foreach (var alreadyExtractedImage in content.Items)
            {
                imagesToDelete.Add(new CDNDeleteImage()
                {
                    ImageName = alreadyExtractedImage.Content
                });
            }
        }
        
        // If there is no items to delete then dont do it!
        if (imagesToDelete.IsNullOrEmpty())
        {
            Console.WriteLine("ImageStorageService: Delete() - no images to delete!");
            return true;
        }
        
        Console.WriteLine($"ImageStorageService: Delete() - PENDING: deleting {imagesToDelete.Count} images");
        var request = new HttpRequestMessage(HttpMethod.Delete, ImageStorageServiceConstants.DELETE_IMAGE)
        {
            Content = new StringContent(JsonConvert.SerializeObject(imagesToDelete), Encoding.UTF8, "application/json")
        };
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string.
            var contentString = await response.Content.ReadAsStringAsync();

            // Deserialize the content string to a boolean.
            bool result = JsonSerializer.Deserialize<bool>(contentString);
            Console.WriteLine($"ImageStorageService: Delete() - SUCCESS: deleting {imagesToDelete.Count} images!");
            return result;
        }
        else
        {
            Console.WriteLine($"ImageStorageService: Delete() - FAIL: deleting {imagesToDelete.Count} images!");
            return false;
        }
    }
}
