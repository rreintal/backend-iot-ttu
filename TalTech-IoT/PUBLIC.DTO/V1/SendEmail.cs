namespace Public.DTO.V1;

public class SendEmail
{
    // TODO - project id which Repository user wants

    public string RecipentEmail { get; set; } = default!;
    
    public string ProjectId { get; set; } = default!;
}