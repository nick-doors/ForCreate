namespace ForCreate.Shared.Entities;

public class CreatedAggregationRoot : AggregationRoot
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}