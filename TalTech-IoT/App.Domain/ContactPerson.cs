using App.Domain.Contracts;
using Base.Domain;

namespace App.Domain;

public class ContactPerson : DomainEntityId, IDomainContentEntity
{
    public string Name { get; set; } = default!;
    public ICollection<Content> Content { get; set; } = default!;
}