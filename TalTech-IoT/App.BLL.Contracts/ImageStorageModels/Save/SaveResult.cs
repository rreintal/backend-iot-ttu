namespace App.BLL.Contracts.ImageStorageModels.Save;

public class SaveResult
{
    public int Sequence { get; set; } = default!;
    public string UpdatedContent { get; set; } = default!; // this is content with already links!
}