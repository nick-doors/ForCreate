using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForCreate.Infrastructure.EmployeeAggregation;

internal class Configuration :
    IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .HasMany<CompanyEmployee>()
            .WithOne(x => x.Employee);

        builder
            .Property("_versionId")
            .HasColumnName("VersionId")
            .IsConcurrencyToken();
    }
}