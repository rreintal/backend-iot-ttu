using Base.Domain;

namespace Public.DTO.V1;

public class GetTopicArea : DomainEntityId
{
    public string Name { get; set; } = default!;
}