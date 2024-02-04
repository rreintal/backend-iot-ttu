using Base.Domain;

namespace DAL.DTO.V1;

public class ContentType : DomainEntityId
{
    public string Name { get; set; } = default!;
}