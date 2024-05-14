using Base.Domain;
using BLL.DTO.ContentHelper;

namespace BLL.DTO.V1;

public class ContactPerson : DomainEntityId, IContentEntity
{
    public string Name { get; set; } = default!;
    public List<Content> Content { get; set; } = default!;
    
}