namespace App.BLL.Contracts.ImageStorageModels.Save;

public class SaveResult
{
    public List<SaveResultItem> Items = new List<SaveResultItem>();
    public List<string> SavedLinks = new List<string>();
}