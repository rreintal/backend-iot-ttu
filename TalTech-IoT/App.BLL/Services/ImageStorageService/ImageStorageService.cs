using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Save.Result;
using App.BLL.Contracts.ImageStorageModels.Update;
using App.BLL.Contracts.ImageStorageModels.Update.Result;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.Domain;
using BLL.DTO.ContentHelper;
using BLL.DTO.Contracts;
using BLL.DTO.ImageService;
using Contracts;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using Public.DTO.Content;

namespace App.BLL.Services.ImageStorageService;


public class ImageStorageService : IImageStorageService
{
    private ImageExtractor _imageExtractor { get; }
    private ImageStorageExecutor _imageStorageExecutor { get; }

    private string IMAGE_PUBLIC_LOCATION { get; set; }

    public ImageStorageService()
    {
        var imagesLocation = Environment.GetEnvironmentVariable("IMAGES_LOCATION");
        if (imagesLocation.IsNullOrEmpty())
        {
            throw new Exception("ImageStorageService: Environemnt variable: IMAGES_LOCATION - is not set or is empty!");
        }

        IMAGE_PUBLIC_LOCATION = imagesLocation!;

        _imageExtractor = new ImageExtractor();
        _imageStorageExecutor = new ImageStorageExecutor();
    }

    public void HandleEntityImageResources<T>(T entity, UpdateImageResources? updateDataResult)
        where T : IContainsImageResource, IDomainEntityId
    {
        if (updateDataResult != null)
        {
            entity.ImageResources = entity.ImageResources
                .Where(image => updateDataResult.DeletedLinks == null || !updateDataResult.DeletedLinks.Contains(image.Link))
                .ToList();

            // Add the SavedLinks
            if (updateDataResult.SavedLinks != null)
            {
                foreach (var link in updateDataResult.SavedLinks)
                {
                    entity.ImageResources.Add(new global::BLL.DTO.V1.ImageResource() { Link = link, NewsId = entity.Id});
                }
            }

            if (updateDataResult.DeletedLinks != null)
            {
                DeleteContent data = new DeleteContent();
                data.Links = updateDataResult.DeletedLinks;
                ProcessDelete(data);
            }
        }
    }

    public SaveImageResources? ProccessSave(object entity)
    {
        var isContentEntity = InstanceOf(entity, typeof(IContentEntity));
        var isImageEntity = InstanceOf(entity, typeof(IContainsImage));
        var isThumbnailEntity = InstanceOf(entity, typeof(IContainsThumbnail));
        var data = new SaveContent() { Items = new List<SaveItem>() };
        if (isContentEntity)
        {
            var bodyEntity = entity as IContentEntity;
            var engBody = ContentHelper.GetContentValue(bodyEntity, ContentTypes.BODY, LanguageCulture.ENG);
            var estBody = ContentHelper.GetContentValue(bodyEntity, ContentTypes.BODY, LanguageCulture.EST);

            var bodyEtPayload = new SaveItem()
            {
                Content = estBody,
                Sequence = 1
            };
            
            var bodyEngPayload = new SaveItem()
            {
                Content = engBody,
                Sequence = 2
            };
            
            data.Items.Add(bodyEtPayload);
            data.Items.Add(bodyEngPayload);
        }

        if (isImageEntity)
        {
            var imageEntity = entity as IContainsImage;
            var image = imageEntity!.Image;
            var imagePayload = new SaveItem()
            {
                Content = image,
                Sequence = 3,
                IsAlreadyBase64 = true
            };
            data.Items.Add(imagePayload);
        }

        if (isThumbnailEntity)
        {
            var thumbnailEntity = entity as IContainsThumbnail;
            var thumbnail = thumbnailEntity!.ThumbnailImage;
            var thumbnailPayload = new SaveItem()
            {
                Content = thumbnail,
                Sequence = 4,
                IsAlreadyBase64 = true
            };
            data.Items.Add(thumbnailPayload);
        }

        var result = Save(data);

        if (result != null)
        {
            if (isContentEntity)
            {
                var etBody = result.Items.FirstOrDefault(e => e.Sequence == 1)?.Content;
                var enBody = result.Items.FirstOrDefault(e => e.Sequence == 2)?.Content;
                if (enBody != null) // TODO: add && imageEntity != null
                {
                    var contentEntity = entity as IContentEntity;
                    ContentHelper.SetContentTranslationValue(contentEntity, ContentTypes.BODY, LanguageCulture.ENG, enBody);
                }
                if (etBody != null) // TODO: add && imageEntity != null
                {
                    var contentEntity = entity as IContentEntity;
                    ContentHelper.SetContentTranslationValue(contentEntity, ContentTypes.BODY, LanguageCulture.EST, etBody);
                }
            }

            if (isImageEntity)
            {
                var imageEntity = entity as IContainsImage;
                var image = result.Items.FirstOrDefault(e => e.Sequence == 3)?.Content;
                if (!image.IsNullOrEmpty()) // TODO: add && imageEntity != null
                {
                    imageEntity!.Image = image;
                }
            }

            if (isThumbnailEntity)
            {
                var thumbnailEntity = entity as IContainsThumbnail;
                var thumbnail = result.Items.FirstOrDefault(e => e.Sequence == 4)?.Content;
                if (thumbnail != null && thumbnailEntity != null)
                {
                    thumbnailEntity.ThumbnailImage = thumbnail;
                }
            }
            return new SaveImageResources()
            {
                SavedLinks = result.SavedLinks
            };
        }


        return null; // TODO: when to return false :)
    }

    public UpdateImageResources? ProccessUpdate(object entity)
    {
        var data = new UpdateContent()
        {
            Items = new List<UpdateItem>()
        };
        var isContentEntity = InstanceOf(entity, typeof(IContentEntity));
        var isImageEntity = InstanceOf(entity, typeof(IContainsImage));
        var isThumbnailEntity = InstanceOf(entity, typeof(IContainsThumbnail));
        if (entity is IContainsImageResource imageResourceEntity)
        {
            data.ExistingImageLinks = imageResourceEntity.ImageResources.Select(e => e.Link).ToList();
        }

        if (entity is IContainsOneImageResource containsOneImageResourceEntity)
        {
            data.ExistingImageLinks = new List<string>() { containsOneImageResourceEntity.ImageResources.Link };
        }
        if (isContentEntity)
        {
            var bodyEntity = entity as IContentEntity;
            var engBody = ContentHelper.GetContentValue(bodyEntity, ContentTypes.BODY, LanguageCulture.ENG);
            var estBody = ContentHelper.GetContentValue(bodyEntity, ContentTypes.BODY, LanguageCulture.EST);

            var bodyEtPayload = new UpdateItem()
            {
                Content = estBody,
                Sequence = 1
            };
            
            var bodyEngPayload = new UpdateItem()
            {
                Content = engBody,
                Sequence = 2
            };
            
            data.Items.Add(bodyEtPayload);
            data.Items.Add(bodyEngPayload);
        }

        if (isImageEntity)
        {
            var imageEntity = entity as IContainsImage;
            var image = imageEntity!.Image;
            // Because for News update, image can be null!
            if (image != null)
            {
                var imagePayload = new UpdateItem()
                {
                    Content = image,
                    Sequence = 3,
                    IsAlreadyBase64 = true
                };
                data.Items.Add(imagePayload);
            }
            else
            {
                isImageEntity = false;
            }
        }

        if (isThumbnailEntity) // If thumbnail is null, then its false!!
        {
            var thumbnailEntity = entity as IContainsThumbnail;
            var thumbnail = thumbnailEntity!.ThumbnailImage;
            // Because for News update, thumbnail can be null!
            if (thumbnail != null)
            {
                var thumbnailPayload = new UpdateItem()
                {
                    Content = thumbnail,
                    Sequence = 4,
                    IsAlreadyBase64 = true
                };
                data.Items.Add(thumbnailPayload);   
            }
            else
            {
                isThumbnailEntity = false;
            }
        }

        var result = Update(data);
        
        if (result != null && !result.IsEmpty())
        {
            if (isContentEntity)
            {
                var etBody = result.Items.FirstOrDefault(e => e.Sequence == 1)?.Content;
                var enBody = result.Items.FirstOrDefault(e => e.Sequence == 2)?.Content;
                if (enBody != null) // TODO: add && imageEntity != null
                {
                    var contentEntity = entity as IContentEntity;
                    ContentHelper.SetContentTranslationValue(contentEntity, ContentTypes.BODY, LanguageCulture.ENG, enBody);
                }
                if (etBody != null) // TODO: add && imageEntity != null
                {
                    var contentEntity = entity as IContentEntity;
                    ContentHelper.SetContentTranslationValue(contentEntity, ContentTypes.BODY, LanguageCulture.EST, etBody);
                }
            }

            if (isImageEntity)
            {
                var imageEntity = entity as IContainsImage;
                var image = result.Items.FirstOrDefault(e => e.Sequence == 3)?.Content;
                if (!image.IsNullOrEmpty() && image != null) // TODO: add && imageEntity != null
                {
                    imageEntity!.Image = image;
                }
            }

            if (isThumbnailEntity)
            {
                var thumbnailEntity = entity as IContainsThumbnail;
                var thumbnail = result.Items.FirstOrDefault(e => e.Sequence == 4)?.Content;
                if (thumbnail != null && thumbnailEntity != null)
                {
                    thumbnailEntity.ThumbnailImage = thumbnail;
                }

            }

            return new UpdateImageResources()
            {
                SavedLinks = result.AddedLinks,
                DeletedLinks = result.DeletedLinks
            };
        }

        return null;

    }

    private SaveResult Save(SaveContent data)
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
        var saveResponseData = _imageStorageExecutor.Upload(CDNPayload); // TODO: TESTS - siia lisa parameeter "test" mis on boolean, kas salvestab FS või ei

        

        var result = new SaveResult();
        // TODO: kui response on null, ehk midagi ei uuendatud, siis lihtsalt returni ka null??
        if (IsSaveResultEmpty(saveResponseData)) return result; // Return empty SaveResult object when nothing to save
            
            
        foreach (var SaveResult in saveResponseData)
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

    public bool ProcessDelete(DeleteContent content)
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
            Console.WriteLine("ImageStorageService: ProcessDelete() - no images to delete!");
            return true;
        }
        
        Console.WriteLine($"ImageStorageService: ProcessDelete() - PENDING: deleting {payload.Count} images");
        var result = _imageStorageExecutor.DeleteImages(payload);
        if (result)
        {
            Console.WriteLine("ImageStorageService: ProcessDelete() - success!");
        }
        else
        {
            Console.WriteLine("ImageStorageService: ProcessDelete() - failure!");
        }
        return result;
    }

    public bool IsBase64(string content)
    {
        return _imageExtractor.IsBase64String(content);
    }

    private UpdateResult? Update(UpdateContent data)
    {
        var existingLinksDuplicate = new List<string>();
        if (!data.ExistingImageLinks.IsNullOrEmpty())
        {
            existingLinksDuplicate = data.ExistingImageLinks;
            // Remove all links which are present in the currently updated content
            // if the link is empty in the end, then all the existing stuff is used
            
            // Reason for this is that: all the links which are associated with some entity (thumbnail, image, content images etc.)
            // are stored in the list. So just checking if the links in new body are different from before is not possible.
            // If the duplicated list is empty in the end it means all of the existing images are still used.
            
            // NEW SOLUTION DOWN
            List<string> linksToRemove = new List<string>();

            foreach (var updateItem in data.Items)
            {
                data.ExistingImageLinks.ForEach(existingLink =>
                {
                    if (updateItem.Content.Contains(existingLink))
                    {
                        // Instead of removing immediately, add to the list of links to remove
                        linksToRemove.Add(existingLink);
                    }
                });
            }
            foreach (var linkToRemove in linksToRemove)
            {
                existingLinksDuplicate.Remove(linkToRemove);
            }
            
            // NEW SOLUTION UP
            var IsNeedToDeleteImages = existingLinksDuplicate.Count != 0;
            if (IsNeedToDeleteImages)
            {
                var DeletePayload = new DeleteContent()
                {
                    Links = existingLinksDuplicate
                };
                var deleteResult = ProcessDelete(DeletePayload);

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


        var saveResult = Save(saveContent);

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
    
    
    private bool InstanceOf(object obj, Type interfaceType)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj), "The object to check cannot be null.");
        }

        if (interfaceType == null)
        {
            throw new ArgumentNullException(nameof(interfaceType), "The interface type cannot be null.");
        }

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("The specified type is not an interface.", nameof(interfaceType));
        }

        return interfaceType.IsAssignableFrom(obj.GetType());
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
