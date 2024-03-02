using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Contracts.ImageStorageModels.Update;
using App.BLL.Contracts.ImageStorageModels.Update.Result;
using App.BLL.Services.ImageStorageService.Models.Delete;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Services.ImageStorageService;

 
public class ImageStorageService : IImageStorageService
{
    private ImageExtractor _imageExtractor { get; }
    private ImageStorageExectutor _imageStorageExecutor { get; }
    
    private string IMAGE_PUBLIC_LOCATION { get; set; } = "http://185.170.213.135:5052/images/"; // TODO: use env variable!

    public ImageStorageService()
    {
        // TODO: use env variable
        var imagesDirectory = Environment.GetEnvironmentVariable("IMAGES_DIRECTORY");
        _imageExtractor = new ImageExtractor();
        _imageStorageExecutor = new ImageStorageExectutor();
    }
    
    public async Task<SaveResult?> Save(SaveContent data)
    {
        var CDNPayload = new CDNSaveImages()
        {
            Items = new List<CDNSaveUnit>()
        };

        
        foreach (var SaveContentItem in data.Items)
        {
            var SaveUnit = new CDNSaveUnit()
            {
                Items = new List<SaveImage>(),
                Sequence = SaveContentItem.Sequence
            };
            
            switch (SaveContentItem.IsAlreadyBase64)
            {
                case true:
                    if (!_imageExtractor.IsBase64String(SaveContentItem.Content)) break;
                    
                    var extractResult = _imageExtractor.CreatePayloadFromBase64(SaveContentItem.Content);
                    
                    // add to payload
                    SaveUnit.Items.Add(new SaveImage()
                    {
                        FileFormat = extractResult!.FileFormat,  // TODO: handle null!!
                        ImageContent = extractResult.ImageContent,
                        Sequence = GetLatestSequence(SaveUnit.Items)
                    });
                    break;
                case false:
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(SaveContentItem.Content);
                    var imgElements = doc.DocumentNode.Descendants("img").ToList();
                    foreach (var imgElement in imgElements)
                    {
                        // Check if image is in base64String or not. as the user can add its own image with preexisting src = link.
                        // Also we use this method for updating but in update the content may contain already images with source
                        var imageSourceInBase64 = imgElement.GetAttributeValue("src", "default value");
                        if(imageSourceInBase64 == "default value" || !_imageExtractor.IsBase64String(imageSourceInBase64)) continue; // This is when the SRC is missing for some reason (should not happen!)
                        var extractorResult = _imageExtractor.CreatePayloadFromBase64(imageSourceInBase64);
                        
                        // add to payload
                        SaveUnit.Items.Add(new SaveImage()
                        {
                            FileFormat = extractorResult!.FileFormat,  // TODO: handle null!!
                            ImageContent = extractorResult.ImageContent,
                            Sequence = GetLatestSequence(SaveUnit.Items)
                        });
                    }
                    break;
            }
            
            CDNPayload.Items.Add(SaveUnit);
        }
        
        // Send items to CDN for save
        // TODO: check here if there is even anything to save!!!!
        var saveResponseData = _imageStorageExecutor.Upload(CDNPayload);

        

        var result = new SaveResult();
        // TODO: kui response on null, ehk midagi ei uuendatud, siis lihtsalt returni ka null??
        if (IsSaveResultEmpty(saveResponseData)) return result; // Return empty SaveResult object when nothing to save
            
            
        foreach (var SaveResult in saveResponseData!)
        {
            var oldContent = data.Items.FirstOrDefault(oldContent => oldContent.Sequence == SaveResult.Sequence);
            if (oldContent == null)
            {
                Console.WriteLine($"ImageStorageService: Save() - Old content with {SaveResult.Sequence} sequence number does not exist.");
                continue;
            }

            // Thumbnail
            if (oldContent.IsAlreadyBase64 && !SaveResult.Items.IsNullOrEmpty())
            {
                var link = CreateImageLink(SaveResult.Items.First().Link); // TODO: Items.First() can theoretically be null!
                // Kui on ainult thumbnail siis kas CDN saadab tagasi seq 0
                result.SavedLinks.Add(link);
                result.Items.Add(new SaveResultItem()
                {
                    Sequence = oldContent.Sequence,
                    Content = link,
                });
                continue;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(oldContent.Content);
            var imgElements = doc.DocumentNode.Descendants("img");

            var sequence = 0;
            foreach (var img in imgElements)
            {
                // HTML structure itself did not change so I can assume that the order from 'doc.DocumentNode.Descendants("img")'.
                // is the same as when filtering form base64
                var isBase64String = IsTagValueInBase64(img);
                if (isBase64String)
                {
                    var imageSaveResult = SaveResult.Items.First(item => item.Sequence == sequence); // TODO: null check!
                    // Add link to the list, in order to save it in db w associtated links to content!
                    // Main reason for it is like this the update MUCH easier!
                    result.SavedLinks.Add(imageSaveResult.Link);
                    
                    var link = CreateImageLink(imageSaveResult.Link);
                    img.SetAttributeValue("src", link);
                    sequence++; // Add sequence only when we are updating any image, otherway it could be that the sequence gets incremented when analyzing some random <img>
                }
                // else continue
            }

            result.Items.Add(new SaveResultItem()
            {
                Sequence = oldContent.Sequence,
                Content = doc.DocumentNode.OuterHtml,
            });
        }

        return result;
    }

    public async Task<bool> Delete(DeleteContent content)
    {
        var payload = new List<CDNDeleteImage>();

        foreach (var link in content.Links)
        {
            var imageName = _imageExtractor.GetImageNameFromLink(link);
            payload.Add(new CDNDeleteImage()
            {
                ImageName = imageName
            });
        }
        // If there are nothing to delete, then end
        if (payload.IsNullOrEmpty())
        {
            Console.WriteLine("ImageStorageService: Delete() - no images to delete!");
            return true;
        }
        
        Console.WriteLine($"ImageStorageService: Delete() - PENDING: deleting {payload.Count} images");
        var result = _imageStorageExecutor.DeleteImages(payload);
        if (result)
        {
            Console.WriteLine("ImageStorageService: Delete() - success!");
        }
        else
        {
            Console.WriteLine("ImageStorageService: Delete() - failure!");
        }
        return result;
    }
    public async Task<UpdateResult?> Update(UpdateContent data)
    {
        
        var existingLinksDuplicate = new List<string>(data.ExistingImageLinks);
        if (!data.ExistingImageLinks.IsNullOrEmpty())
        {
            // Remove all links which are present in the currently updated content
            // if the link is empty in the end, then all the existing stuff is used
            
            // Reason for this is that: all the links which are associated with some entity (thumbnail, image, content images etc.)
            // are stored in the list. So just checking if the links in new body are different from before is not possible.
            // If the duplicated list is empty in the end it means all of the existing images are still used.
            foreach (var updateItem in data.Items)
            {
                data.ExistingImageLinks.ForEach(existingLink =>
                {
                    if (updateItem.Content.Contains(existingLink))
                    {
                        existingLinksDuplicate.Remove(existingLink);
                    }
                });
            }

            var IsNeedToDeleteImages = existingLinksDuplicate.Count != 0;
            if (IsNeedToDeleteImages)
            {
                var DeletePayload = new DeleteContent()
                {
                    Links = existingLinksDuplicate
                };
                var deleteResult = await Delete(DeletePayload);

                if (deleteResult == false)
                {
                    // TODO: what to do here?
                    // Show some error that the delete was not successful??
                }
            }
        }

        var saveContent = new SaveContent()
        {
            Items = data.Items.Select(updateItem =>
            {
                return new SaveItem()
                {
                    Sequence = updateItem.Sequence,
                    Content = updateItem.Content,
                    IsAlreadyBase64 = updateItem.IsAlreadyBase64
                };
            }).ToList()
        };


        var saveResult = await Save(saveContent);

        if (saveResult != null)
        {
            var updateResult = new UpdateResult()
            {
                AddedLinks = saveResult.SavedLinks,
                DeletedLinks = existingLinksDuplicate.IsNullOrEmpty() ? null : existingLinksDuplicate, // Here are the links which are deleted
                Items = saveResult.Items.Select(e =>
                { 
                    return new UpdateResultItem()
                    {
                        Content = e.Content,
                        Sequence = e.Sequence
                    };
                }).ToList()
            };
            return updateResult;   
        }

        // TODO: mõtle mis oleks parem nulli asemel
        return null;
    }
    
    // Helpers
    
    private bool IsSaveResultEmpty(List<CDNSaveResult> items)
    {
        foreach (var set in items)
        {
            if (!set.Items.IsNullOrEmpty()) return false;
        }

        return true;
    }
    
    
    private string CreateImageLink(string link)
    {
        return $"{IMAGE_PUBLIC_LOCATION}{link}";
    }

    private bool IsTagValueInBase64(HtmlNode imgTag)
    {
        var defaultValue = "DEF";
        var value = imgTag.GetAttributeValue("src", defaultValue);
        if (value == defaultValue)
        {
            Console.WriteLine("ImageStorageService: IsTagValueInBase64 - Failed to get 'src' attribute value");
            return false;
        }
        return _imageExtractor.IsBase64String(value);
    }

    private int GetLatestSequence(List<SaveImage> list)
    {
        if (list.Count == 0)
        {
            return 0;
        }
        int latestSequence = list.Max(image => image.Sequence) + 1;
        return latestSequence;
    }


}
