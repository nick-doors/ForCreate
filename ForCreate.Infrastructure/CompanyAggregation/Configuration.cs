using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForCreate.Infrastructure.CompanyAggregation;

internal class Configuration :
    IEntityTypeConfiguration<Company>,
    IEntityTypeConfiguration<CompanyEmployee>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
            .HasMany(x => x.Employees)
            .WithOne(x => x.Company);

        builder
            .Property("_versionId")
            .HasColumnName("VersionId")
            .IsConcurrencyToken();
    }

    public void Configure(EntityTypeBuilder<CompanyEmployee> builder)
    {
        builder
            .Property<Guid>("CompanyId");
        builder
            .Property<Guid>("EmployeeId");
        builder
            .HasKey(
                $"{nameof(CompanyEmployee.Company)}{nameof(Company.Id)}",
                $"{nameof(CompanyEmployee.Employee)}{nameof(Employee.Id)}");
    }
}