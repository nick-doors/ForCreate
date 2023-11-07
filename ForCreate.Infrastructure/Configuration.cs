using System.Runtime.CompilerServices;
using ForCreate.Shared.Data;
using ForCreate.Shared.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("ForCreate.Infrastructure.Tests")]
namespace ForCreate.Infrastructure;

public static class Configuration
{
    public static void AddInfrastructure(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ForCreateDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork<ForCreateDbContext>>();
        services.AddSingleton<IDapperConnectionFactory>(_ => new DapperConnectionFactory(connectionString));
        services
            .Scan(s => s
                .FromAssemblies(typeof(Configuration).Assembly)
                .AddClasses(c => c.AssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddScoped(_ => ResiliencyPolicy.GetSqlResiliencyPolicy());
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ForCreateDbContext>();
        context.Database.Migrate();
    }
}