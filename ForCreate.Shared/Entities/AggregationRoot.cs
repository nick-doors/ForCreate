using ForCreate.Shared.Events;

namespace ForCreate.Shared.Entities;

public abstract class AggregationRoot :
    Entity,
    IAggregationRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    private int _versionId;

    public virtual void IncreaseVersion()
    {
        _versionId++;
    }

    private readonly LinkedList<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Domain events occurred.
    /// </summary>
    public IEnumerable<IDomainEvent> GetDomainEvents() => _domainEvents;

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.AddLast(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public IEnumerable<IIntegrationEvent> GetIntegrationEvents()
    {
        return _domainEvents
            .Select(ConvertEvent)
            .Where(integrationEvent => integrationEvent != null)
            .Select(x => x!);
    }

    protected virtual IIntegrationEvent? ConvertEvent<TDomainEvent>(TDomainEvent domainEvent)
        where TDomainEvent : IDomainEvent
    {
        return null;
    }
}