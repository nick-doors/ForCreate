using ForCreate.Shared.Entities;

namespace ForCreate.Shared.Events;

public abstract record AggregationRootCreatedEvent<TEntity>(TEntity Entity) : DomainEvent
    where TEntity : AggregationRoot;