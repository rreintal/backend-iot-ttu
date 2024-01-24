namespace Public.DTO.Identity;

public class AddRole
{
    public Guid UserId { get; set; } = default!;
    public string Role { get; set; } = default!;
}