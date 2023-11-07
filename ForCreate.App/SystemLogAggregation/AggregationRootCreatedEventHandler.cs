using ForCreate.Core.Enums;
using ForCreate.Core.SystemLogAggregation;
using ForCreate.Shared.Entities;
using ForCreate.Shared.Events;
using MediatR;

namespace ForCreate.App.SystemLogAggregation;

internal abstract class AggregationRootCreatedEventHandler<TEvent, TEntity> :
    INotificationHandler<TEvent>
    where TEvent : AggregationRootCreatedEvent<TEntity>
    where TEntity : AggregationRoot
{
    private readonly ISystemLogRepository _systemLogRepository;

    protected AggregationRootCreatedEventHandler(ISystemLogRepository systemLogRepository)
    {
        _systemLogRepository = systemLogRepository;
    }

    protected abstract string ResourceName(TEntity entity);

    public Task Handle(
        TEvent notification,
        CancellationToken cancellationToken)
    {
        var systemLog = SystemLog.Create(notification.Entity, EventType.Create,
            $"New {typeof(TEntity).Name} '{ResourceName(notification.Entity)}' was created");
        _systemLogRepository.Add(systemLog);
        return Task.CompletedTask;
    }
}