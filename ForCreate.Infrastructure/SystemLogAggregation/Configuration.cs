using ForCreate.Core.SystemLogAggregation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForCreate.Infrastructure.SystemLogAggregation;

internal class Configuration :
    IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}