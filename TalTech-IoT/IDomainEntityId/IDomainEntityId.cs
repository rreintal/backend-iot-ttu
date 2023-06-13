namespace IDomainEntityId;

public interface IDomainEntityId<TKey> 
{
    public TKey Id { get; set; }
}