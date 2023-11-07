using MediatR;

namespace ForCreate.Shared.Events;

public interface IIntegrationEvent : INotification
{
    DateTime OccurredOn { get; }
}