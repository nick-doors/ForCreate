using ForCreate.Core.SystemLogAggregation;
using ForCreate.Shared.Infrastructure;

namespace ForCreate.Infrastructure.SystemLogAggregation;

internal class SystemLogRepository : Repository<ForCreateDbContext, SystemLog>, ISystemLogRepository
{
    public SystemLogRepository(
        ForCreateDbContext context)
        : base(context)
    {
    }
}