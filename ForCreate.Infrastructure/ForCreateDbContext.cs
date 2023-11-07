using Microsoft.EntityFrameworkCore;

namespace ForCreate.Infrastructure;

internal class ForCreateDbContext : DbContext
{
    public ForCreateDbContext(
        DbContextOptions<ForCreateDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForCreateDbContext).Assembly);
    }
}