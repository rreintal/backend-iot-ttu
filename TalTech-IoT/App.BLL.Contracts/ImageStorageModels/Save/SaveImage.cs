namespace App.BLL.Contracts.ImageStorageModels.Save;

public class SaveImage
{
    public string FileFormat { get; set; } = default!;
    public string ImageContent { get; set; } = default!;
    public int Sequence { get; set; } = default!;
}