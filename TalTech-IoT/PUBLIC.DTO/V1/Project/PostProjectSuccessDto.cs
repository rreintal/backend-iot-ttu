namespace Public.DTO.V1;

public class PostProjectSuccessDto : PostProjectDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}