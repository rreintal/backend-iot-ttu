using Base.Domain;

namespace BLL.DTO.V1;

public class ContentType : DomainEntityId
{
    public string Name { get; set; } = default!;
}