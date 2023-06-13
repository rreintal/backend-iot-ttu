using Contracts;
namespace Base.Domain;

public abstract class DomainEntityId : IDomainEntityId
{
    public Guid Id { get; set; } = Guid.NewGuid();
}