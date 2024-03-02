using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Contracts.ImageStorageModels.Update.Result;

public class UpdateResult
{
    public List<UpdateResultItem> Items { get; set; } = default!;
    public List<string> AddedLinks { get; set; } = default!;
    public List<string>? DeletedLinks { get; set; } = default!;

    public bool IsEmpty()
    {
        return Items.IsNullOrEmpty();
    }
}