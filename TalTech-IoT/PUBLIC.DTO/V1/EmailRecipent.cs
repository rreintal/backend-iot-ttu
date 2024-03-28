namespace Public.DTO.V1;

public class EmailRecipent
{
    public Guid? Id { get; set; }
    public string Email { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? Comment { get; set; } = default!;
}