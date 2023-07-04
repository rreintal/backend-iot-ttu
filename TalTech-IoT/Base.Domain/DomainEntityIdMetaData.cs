using Contracts;

namespace Base.Domain;

public abstract class DomainEntityIdMetaData : IDomainEntityIdMetaData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}