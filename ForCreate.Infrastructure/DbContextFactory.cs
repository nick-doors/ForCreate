using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ForCreate.Infrastructure;

internal class DbContextFactory : IDesignTimeDbContextFactory<ForCreateDbContext>
{
    ForCreateDbContext IDesignTimeDbContextFactory<ForCreateDbContext>.CreateDbContext(
        string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ForCreateDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost;Database=ForCreate;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;");

        return new ForCreateDbContext(optionsBuilder.Options);
    }
}