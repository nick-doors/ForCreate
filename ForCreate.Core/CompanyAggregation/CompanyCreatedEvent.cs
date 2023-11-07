using ForCreate.Shared.Events;

namespace ForCreate.Core.CompanyAggregation;

public record CompanyCreatedEvent(Company Entity) : 
    AggregationRootCreatedEvent<Company>(Entity);