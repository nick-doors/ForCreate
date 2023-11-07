using Dapper;
using ForCreate.Core.CompanyAggregation;
using ForCreate.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ForCreate.Infrastructure.CompanyAggregation;

internal class CompanyRepository :
    AggregationRootRepository<ForCreateDbContext, Company>,
    ICompanyRepository
{
    private readonly IDapperConnectionFactory _connectionFactory;

    public CompanyRepository(
        IAsyncPolicy resiliencyPolicy,
        ForCreateDbContext context,
        IDapperConnectionFactory connectionFactory)
        : base(context, resiliencyPolicy)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CompanyByNameDto?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var result = await ResiliencyPolicy.ExecuteAsync(
            async ct =>
            {
                await using var connection = await _connectionFactory.CreateConnectionAsync(ct);

                var company = await connection.QueryFirstOrDefaultAsync<CompanyByNameDto>(new CommandDefinition(
                    """
                    SELECT
                        [c].[Id] AS [Id]
                    FROM
                        [Company] [c]
                    WHERE
                        @name = [c].[Name];
                    """,
                    new { name = name },
                    cancellationToken: ct));

                return company;
            },
            cancellationToken);

        return result;
    }
        
    protected override IQueryable<Company> Include(IQueryable<Company> companies)
    {
        return companies
            .Include(x => x.Employees);
    }
}