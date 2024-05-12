using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class ContactPerson : DomainEntityId
{
    [MinLength(2)]
    [MaxLength(64)]
    public string Name { get; set; } = default!;
    [IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
}