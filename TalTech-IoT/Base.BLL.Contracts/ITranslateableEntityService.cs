using Base.DAL.EF.Contracts;
using Contracts;

namespace Base.BLL.Contracts;

public interface ITranslateableEntityService<TEntity> : ITranslateableEntityService<TEntity, Guid>, IEntityService<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
    
}

public interface ITranslateableEntityService<TEntity, TKey> : IBaseTranslateableRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    
}