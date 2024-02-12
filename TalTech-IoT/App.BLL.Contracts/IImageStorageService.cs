
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Services.ImageStorageService.Models.Delete;

namespace App.BLL.Contracts;

public interface IImageStorageService
{
    public Task<SaveResult> Save(SaveContent data);
    public string ReplaceImages(string content);
    
    public bool Delete(string content);

}