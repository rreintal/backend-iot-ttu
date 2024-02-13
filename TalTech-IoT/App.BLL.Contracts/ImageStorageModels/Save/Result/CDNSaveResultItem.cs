namespace App.BLL.Contracts.ImageStorageModels.Save.Result;

public class CDNSaveResultItem
{
    public int Sequence { get; set; } = default!;
    public string Link { get; set; } = default!;
    public bool IsRawImage { get; set; }
}