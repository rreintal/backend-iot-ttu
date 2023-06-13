namespace Base.Domain;

public abstract class DomainEntityId : IDomainEntityId<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public interface IDomainEntityId<TKey> 
{
    public TKey Id { get; set; }
}