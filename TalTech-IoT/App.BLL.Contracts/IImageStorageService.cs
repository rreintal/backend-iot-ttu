
using App.BLL.Contracts.ImageStorageModels.Save;
using App.BLL.Contracts.ImageStorageModels.Update;
using App.BLL.Services.ImageStorageService.Models.Delete;

namespace App.BLL.Contracts;

public interface IImageStorageService
{
    public Task<List<SaveResult>> Save(SaveContent data);
    public Task<bool> Delete(DeleteContent content, bool alreadyImages = false);

    public Task<List<UpdateResult>> Update(UpdateContent data);

}