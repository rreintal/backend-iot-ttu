﻿using AutoMapper;
using Base.DAL.EF.Contracts;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EFBaseRepository<TEntity, TDbContext> : EFBaseRepository<TEntity, Guid, TDbContext>, IBaseRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class, IDomainEntityId
{
    public EFBaseRepository(TDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }
}

public class EFBaseRepository<TEntity, TKey, TDbContext> : IBaseRepository<TEntity, TKey> 
    where TKey : struct, IEquatable<TKey> 
    where TEntity : class, IDomainEntityId<TKey>
    where TDbContext : DbContext
{
    protected TDbContext DbContext;
    protected DbSet<TEntity> DbSet;
    protected IMapper _mapper;

    public EFBaseRepository(TDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        
        // Disable tracking globally
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        _mapper = mapper;
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> AllAsync()
    {
         return await DbSet.ToListAsync();
    }

    public virtual TEntity? Find(Guid id)
    {
        return DbSet.Find(id);
    }

    public virtual async Task<TEntity?> FindAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual TEntity Add(TEntity entity)
    {
        return DbSet.Add(entity).Entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        return DbSet.Update(entity).Entity;
    }

    public virtual TEntity Remove(TEntity entity)
    {
        return DbSet.Remove(entity).Entity;
    }

    public async virtual Task<TEntity?> RemoveAsync(TKey id)
    {
        var entity = await DbSet.FindAsync(id);
        return DbSet.Remove(entity).Entity;
    }
}