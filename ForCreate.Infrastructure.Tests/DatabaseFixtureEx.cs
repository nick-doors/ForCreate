using ForCreate.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ForCreate.Infrastructure.Tests;

internal static class DatabaseFixtureEx
{
    public static ForCreateDbContext Context(this DatabaseFixture fixture)
    {
        var options = new DbContextOptionsBuilder<ForCreateDbContext>()
            .UseSqlServer(fixture.ConnectionString).Options;

        return new ForCreateDbContext(options);
    }

    public static DapperConnectionFactory Dapper(this DatabaseFixture fixture)
    {
        return new DapperConnectionFactory(fixture.ConnectionString);
    }
}