using AutoMapper;
using Contracts;

namespace Base.DAL.EF.Contracts;

public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
where TEntity : class, IDomainEntityId
{
}


public interface IBaseRepository<TEntity, in TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public string? languageCulture { get; set; }
    Task<IEnumerable<TEntity>> AllAsync();

    Task<TEntity?> FindAsync(TKey id);

    TEntity Add(TEntity entity);
    
    TEntity Update(TEntity entity);

    TEntity Remove(TEntity entity);

    Task<TEntity?> RemoveAsync(TKey id);
}