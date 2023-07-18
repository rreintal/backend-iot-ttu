using Contracts;

namespace Base.Domain;

public abstract class DomainEntityIdMetaData : IDomainEntityIdMetaData, IDomainEntityId
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}