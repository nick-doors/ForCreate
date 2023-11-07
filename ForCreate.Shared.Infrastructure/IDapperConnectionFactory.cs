using System.Data.Common;

namespace ForCreate.Shared.Infrastructure;

public interface IDapperConnectionFactory
{
    Task<DbConnection> CreateConnectionAsync(
        CancellationToken cancellationToken);
}