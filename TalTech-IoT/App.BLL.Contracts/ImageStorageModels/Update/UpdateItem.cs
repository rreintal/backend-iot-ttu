namespace App.BLL.Contracts.ImageStorageModels.Update;

public class UpdateItem
{
    public string Content { get; set; } = default!;
    public int Sequence { get; set; } = default!;
    public bool IsAlreadyBase64 = false;
}