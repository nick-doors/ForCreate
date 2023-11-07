using ForCreate.Shared.Data;
using ForCreate.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForCreate.Shared.Infrastructure;

public abstract class Repository<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : Entity
{
    protected TDbContext Context { get; }

    protected Repository(TDbContext context)
    {
        Context = context;
    }

    public void Add(TEntity entity)
    {
        Context.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Context.AddRange(entities);
    }
}