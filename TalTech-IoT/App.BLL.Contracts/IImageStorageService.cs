
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Update;
using App.BLL.Contracts.ImageStorageModels.Update.Result;
using App.BLL.Services.ImageStorageService.Models.Delete;

namespace App.BLL.Contracts;

public interface IImageStorageService
{
    public SaveResult? Save(SaveContent data);
    public bool Delete(DeleteContent content);

    public UpdateResult? Update(UpdateContent data);

}