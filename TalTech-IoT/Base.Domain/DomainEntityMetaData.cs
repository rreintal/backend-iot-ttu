using Contracts;

namespace Base.Domain;

public abstract class DomainEntityMetaData : IDomainEntityMetaData
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}