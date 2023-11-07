using ForCreate.Core.Enums;
using ForCreate.Shared.Entities;
using Newtonsoft.Json;

namespace ForCreate.Core.SystemLogAggregation;

public class SystemLog : Entity
{
    private SystemLog()
    {
            
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string ResourceType { get; private set; } = default!;

    public Guid ResourceId { get; private set; }

    public EventType Event { get; private set; }

    public string Resource { get; private set; } = default!;

    public string Comment { get; private set; } = default!;

    public static SystemLog Create<TEntity>(TEntity entity,
        EventType @event,
        string comment)
        where TEntity : AggregationRoot
    {
        return new SystemLog
        {
            ResourceType = typeof(TEntity).Name,
            ResourceId = entity.Id,
            Resource = JsonConvert.SerializeObject(
                entity,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
            Event = @event,
            Comment = comment
        };
    }
}