using Contracts;

namespace Base.DAL.EF.Contracts;

public interface IBaseTranslateableRepository<TEntity> : IBaseTranslateableRepository<TEntity, Guid>, IBaseRepository<TEntity>
where TEntity : class, IDomainEntityId
{
    
}

public interface IBaseTranslateableRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
where TKey: struct, IEquatable<TKey>
where TEntity : class, IDomainEntityId<TKey>
{
    Task<IEnumerable<TEntity>> AllAsync(string? languageCulture);
    Task<TEntity?> FindAsync(TKey id, string? languageCulture);
}