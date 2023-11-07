using ForCreate.Shared.Data;
using ForCreate.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ForCreate.Shared.Infrastructure;

public abstract class AggregationRootRepository<TDbContext, TEntity>
    : Repository<TDbContext, TEntity>, IRootRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : AggregationRoot
{
    protected IAsyncPolicy ResiliencyPolicy { get; }

    protected AggregationRootRepository(
        TDbContext context,
        IAsyncPolicy resiliencyPolicy)
        : base(context)
    {
        ResiliencyPolicy = resiliencyPolicy;
    }

    public virtual async Task<ICollection<TEntity>> ListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var result = await ResiliencyPolicy.ExecuteAsync(
            async ct =>
            {
                var entitiesQuery = Context.Set<TEntity>()
                    .Where(x => ids.Contains(x.Id));

                entitiesQuery = Include(entitiesQuery);

                return await entitiesQuery
                    .ToListAsync(ct);
            },
            cancellationToken);

        return result;
    }

    protected virtual IQueryable<TEntity> Include(
        IQueryable<TEntity> entities)
    {
        return entities;
    }
}