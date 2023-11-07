using Dapper;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Shared.Infrastructure;
using Polly;

namespace ForCreate.Infrastructure.EmployeeAggregation;

internal class EmployeeRepository :
    AggregationRootRepository<ForCreateDbContext, Employee>,
    IEmployeeRepository
{
    private readonly IDapperConnectionFactory _connectionFactory;

    public EmployeeRepository(
        ForCreateDbContext context,
        IDapperConnectionFactory connectionFactory,
        IAsyncPolicy resiliencyPolicy)
        : base(context, resiliencyPolicy)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<EmployeeByEmailDto>> GetByEmailAsync(
        IEnumerable<string> emails,
        CancellationToken cancellationToken = default)
    {
        var enlisted = emails
            .ToHashSet();

        if (!enlisted.Any())
            return Array.Empty<EmployeeByEmailDto>();

        var result = await ResiliencyPolicy.ExecuteAsync(
            async ct =>
            {
                await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var employees = await connection.QueryAsync<EmployeeByEmailDto>(new CommandDefinition(
                    """
                    SELECT
                        [e].[Id] AS [Id],
                        [e].[Email] AS [Email]
                    FROM
                        [Employee] [e]
                    WHERE
                        [e].[Email] IN @emails
                    """,
                    new { emails = enlisted },
                    cancellationToken: ct));

                return employees;
            },
            cancellationToken);

        return result;
    }
}