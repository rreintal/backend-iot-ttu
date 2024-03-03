namespace App.BLL.Contracts.ImageStorageModels.Save;

public class SaveResultItem
{
    public int Sequence { get; set; } = default!;
    public string Content { get; set; } = default!; // this is content with already links!
    
}