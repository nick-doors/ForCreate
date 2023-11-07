using Microsoft.Extensions.DependencyInjection;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ForCreate.App.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ForCreate.App;

public static class Configuration
{
    public static void AddApp(this IServiceCollection services)
    {
        services.AddScoped<ICompanyEmployeeService, CompanyEmployeeService>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
    }
}