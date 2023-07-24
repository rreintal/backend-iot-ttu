using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.EF.Contracts;
using Contracts;

namespace Base.BLL;

public class BaseEntityService<TBllEntity, TDalEntity, TRepository> :
    BaseEntityService<TBllEntity, TDalEntity, TRepository, Guid>, IEntityService<TBllEntity> 
    where TBllEntity : class, IDomainEntityId 
    where TDalEntity : class, IDomainEntityId
    where TRepository : class, IBaseRepository<TDalEntity>
{
    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository, mapper)
    {
    }
}

public class BaseEntityService<TBllEntity, TDalEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey> 
    where TDalEntity : class, IDomainEntityId<TKey>
    where TRepository : class, IBaseRepository<TDalEntity, TKey>
    where TKey : struct, IEquatable<TKey>
{
    public string? languageCulture { get; set; }
    protected readonly TRepository Repository;
    protected readonly IMapper<TBllEntity, TDalEntity> Mapper;

    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public void SetLanguageStrategy(string languageCulture)
    {
        Repository.languageCulture = languageCulture;
    }

    public async Task<IEnumerable<TBllEntity>> AllAsync()
    {
        return (await Repository.AllAsync()).Select(e => Mapper.Map(e));
    }

    public async Task<TBllEntity?> FindAsync(TKey id)
    {
        return Mapper.Map(await Repository.FindAsync(id));
    }

    public TBllEntity Add(TBllEntity entity)
    {
        return Mapper.Map(Repository.Add(Mapper.Map(entity)))!;
    }

    public TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)))!;
    }

    public TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(Repository.Remove(Mapper.Map(entity)))!;
    }

    public async Task<TBllEntity?> RemoveAsync(TKey id)
    {
        return Mapper.Map(await Repository.RemoveAsync(id));
    }
}