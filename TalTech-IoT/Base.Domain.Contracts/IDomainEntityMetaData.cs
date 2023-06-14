namespace Contracts;

public interface IDomainEntityMetaData : IDomainEntityMetaData<DateTime>
{
    
}

public interface IDomainEntityMetaData<TDate>
where TDate : struct
{
    TDate CreatedAt { get; set; }
}