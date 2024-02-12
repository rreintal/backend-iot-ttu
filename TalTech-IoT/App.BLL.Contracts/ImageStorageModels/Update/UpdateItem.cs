namespace App.BLL.Contracts.ImageStorageModels.Update;

public class UpdateItem
{
    public string OldContent { get; set; } = default!;
    public string NewContent { get; set; } = default!;
    public int Sequence { get; set; } = default!;
}