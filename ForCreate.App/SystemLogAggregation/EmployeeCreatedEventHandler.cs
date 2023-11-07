using ForCreate.Core.EmployeeAggregation;
using ForCreate.Core.SystemLogAggregation;

namespace ForCreate.App.SystemLogAggregation;

internal sealed class EmployeeCreatedEventHandler :
    AggregationRootCreatedEventHandler<EmployeeCreatedEvent, Employee>
{
    public EmployeeCreatedEventHandler(ISystemLogRepository systemLogRepository)
        : base(systemLogRepository)
    {
    }

    protected override string ResourceName(Employee entity) => entity.Email;
}