using ForCreate.Shared.Entities;

namespace ForCreate.Shared.Data;

public interface IRootRepository<TEntity> :
    IRepository<TEntity>
    where TEntity : AggregationRoot
{
    Task<ICollection<TEntity>> ListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}