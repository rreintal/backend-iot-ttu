using App.BLL.Services.ImageStorageService.Models.Delete;

namespace App.BLL.Contracts;

public interface IImageStorageService
{
    public bool ProccessSave(Object entity);

    public void ProccessUpdate(Object entity);
    public void ProccessDelete(Object entity);
    public bool Delete(DeleteContent content);

    public bool IsBase64(string content);

}