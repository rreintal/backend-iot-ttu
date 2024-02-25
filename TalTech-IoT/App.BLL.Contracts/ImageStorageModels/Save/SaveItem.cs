namespace App.BLL.Contracts.ImageStorageModels.Save;

public class SaveItem
{
    public int Sequence { get; set; } = default!;
    public string Content { get; set; } = default!;

    public bool IsAlreadyBase64 = false; // if it is not inside content!
}