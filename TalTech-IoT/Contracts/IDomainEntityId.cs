namespace Contracts;

public interface IDomainEntityId : IDomainEntityId<Guid>
{
    
}


public interface IDomainEntityId<TKey> 
where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
}