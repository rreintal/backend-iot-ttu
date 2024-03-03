namespace App.BLL.Contracts.ImageStorageModels.Update;

public class UpdateContent
{
    public List<UpdateItem> Items { get; set; } = default!;
    public List<string> ExistingImageLinks { get; set; } = default!;
}