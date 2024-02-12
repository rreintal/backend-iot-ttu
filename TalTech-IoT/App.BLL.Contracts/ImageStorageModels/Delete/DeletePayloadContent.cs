namespace App.BLL.Services.ImageStorageService.Models.Delete;

public class DeletePayloadContent
{
    public string OldContent { get; set; } = default!;
    public string NewContent { get; set; } = default!;
}