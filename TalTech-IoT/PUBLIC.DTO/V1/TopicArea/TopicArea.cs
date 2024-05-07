using Base.Domain;
namespace Public.DTO.V1;

public class TopicArea : DomainEntityId
{
    public TopicArea()
    {
    }

    public TopicArea(Guid id)
    {
        Id = id;
    }

    public string? Name { get; set; }
    
}

