using System.Data.SqlClient;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Testcontainers.SqlEdge;

namespace ForCreate.Infrastructure.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly SqlEdgeContainer _container;
    private readonly Lazy<string> _connectionString;

    public string ConnectionString => _connectionString.Value;

    public DatabaseFixture()
    {
        _container = new SqlEdgeBuilder()
            .Build();

        _connectionString = new Lazy<string>(() =>
        {
            var dbName = Guid.NewGuid().ToString("N");
            var builder = new SqlConnectionStringBuilder(_container.GetConnectionString())
            {
                InitialCatalog = dbName
            };
            return builder.ConnectionString;
        });
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await SeedAsync();
    }

    public Employee[] Employees { get; } =
    {
        Employee.Create("email_1@domain.com", EmployeeTitle.Developer),
        Employee.Create("email_2@domain.com", EmployeeTitle.Tester)
    };

    private async Task SeedAsync()
    {
        var options = new DbContextOptionsBuilder<ForCreateDbContext>()
            .UseSqlServer(ConnectionString).Options;

        await using var context = new ForCreateDbContext(options);

        await context.Database.EnsureCreatedAsync();

        context.Set<Employee>()
            .AddRange(Employees);

        await context.SaveChangesAsync();
    }
}