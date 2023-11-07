using ForCreate.Shared.Events;

namespace ForCreate.Core.EmployeeAggregation;

public record EmployeeCreatedEvent(Employee Entity) : 
    AggregationRootCreatedEvent<Employee>(Entity);