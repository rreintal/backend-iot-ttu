namespace App.BLL.Contracts.ImageStorageModels.Save.Result;

public class CDNSaveResult
{
    public int Sequence { get; set; } = default!;
    public List<CDNSaveResultItem> Items { get; set; } = default!;
    
}