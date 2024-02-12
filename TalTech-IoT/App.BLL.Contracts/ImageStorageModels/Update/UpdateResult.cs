namespace App.BLL.Contracts.ImageStorageModels.Update;

public class UpdateResult
{
    public int Sequence { get; set; } = default!;
    public string NewContent { get; set; } = default!;
}