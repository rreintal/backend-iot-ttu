using Base.Domain;

namespace Public.DTO.V1;

public class GetContactPerson : DomainEntityId
{
    public string Name { get; set; } = default!;
    public string Body { get; set; } = default!;
}