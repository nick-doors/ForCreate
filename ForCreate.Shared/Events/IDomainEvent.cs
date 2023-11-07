using MediatR;

namespace ForCreate.Shared.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}