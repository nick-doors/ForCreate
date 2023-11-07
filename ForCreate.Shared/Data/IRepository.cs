using ForCreate.Shared.Entities;

namespace ForCreate.Shared.Data;

public interface IRepository
{
}

public interface IRepository<in TEntity> :
    IRepository
    where TEntity : Entity
{
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);
}