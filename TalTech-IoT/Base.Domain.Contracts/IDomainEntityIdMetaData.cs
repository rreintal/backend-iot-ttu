namespace Contracts;

public interface IDomainEntityIdMetaData : IDomainEntityIdMetaData<Guid, DateTime>
{
    
}

public interface IDomainEntityIdMetaData<TKey, TDate>
where TKey : struct, IEquatable<TKey>
where TDate : IComparable 
{
    TKey Id { get; set; }
    TDate CreatedAt { get; set; }
    
}