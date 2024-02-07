using Base.Domain;

namespace Public.DTO.V1;

public class ContactPerson : DomainEntityId
{
    public string Name { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;
}