using App.BLL.Services.ImageStorageService.Models.Delete;
using BLL.DTO.Contracts;
using BLL.DTO.ImageService;
using Contracts;

namespace App.BLL.Contracts;

public interface IImageStorageService
{

    public bool IsBase64String(string input);
    public void HandleEntityImageResources<T>(T entity, UpdateImageResources updateDataResult)
        where T : IContainsImageResource, IDomainEntityId;
    
    public SaveImageResources? ProccessSave(Object entity, bool test = false);


    public UpdateImageResources? ProccessUpdate(Object entity);
    public bool ProcessDelete(DeleteContent content);
    
}