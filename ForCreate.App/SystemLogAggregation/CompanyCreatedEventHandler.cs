using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.SystemLogAggregation;

namespace ForCreate.App.SystemLogAggregation;

internal sealed class CompanyCreatedEventHandler
    : AggregationRootCreatedEventHandler<CompanyCreatedEvent, Company>
{
    public CompanyCreatedEventHandler(ISystemLogRepository systemLogRepository)
        : base(systemLogRepository)
    {
    }

    protected override string ResourceName(Company entity) => entity.Name;
}