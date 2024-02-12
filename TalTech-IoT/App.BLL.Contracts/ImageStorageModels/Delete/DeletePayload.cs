namespace App.BLL.Services.ImageStorageService.Models.Delete;

public class DeletePayload
{
    public List<DeletePayloadContent> Data { get; set; } = default!;
}