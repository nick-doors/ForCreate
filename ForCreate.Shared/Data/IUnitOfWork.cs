namespace ForCreate.Shared.Data;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}