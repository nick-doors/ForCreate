using ForCreate.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.SqlEdge;

namespace ForCreate.Tests.Controllers;

public class BaseIntegrationTestsFactory : WebApplicationFactory<EmployeeController>,
    IAsyncLifetime
{
    private readonly SqlEdgeContainer _container;
    private readonly Lazy<string> _connectionString;

    private string ConnectionString => _connectionString.Value;

    public BaseIntegrationTestsFactory()
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

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configBuilder =>
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ForCreateDb"] = ConnectionString
                }));
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}