using App.BLL.Services.ImageStorageService.Models.Delete;
using BLL.DTO.ImageService;

namespace App.BLL.Contracts;

public interface IImageStorageService
{
    public SaveImageResources? ProccessSave(Object entity);

    public UpdateImageResources? ProccessUpdate(Object entity);
    public bool ProcessDelete(DeleteContent content);

    public bool IsBase64(string content);

}