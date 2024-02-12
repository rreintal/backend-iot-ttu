namespace App.BLL.Contracts.ImageStorageModels.Save;

public class CDNSaveUnit
{
    public int Sequence { get; set; } = default!;
    public List<SaveImage> Items { get; set; } = default!;
}