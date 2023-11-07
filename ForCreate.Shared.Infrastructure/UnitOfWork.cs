using ForCreate.Shared.Data;
using ForCreate.Shared.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ForCreate.Shared.Infrastructure;

public class UnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly IAsyncPolicy _resiliencyPolicy;
    private readonly IPublisher _publisher;

    public UnitOfWork(
        TDbContext context,
        IAsyncPolicy resiliencyPolicy,
        IPublisher publisher)
    {
        _context = context;
        _resiliencyPolicy = resiliencyPolicy;
        _publisher = publisher;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        var aggregationRoots = _context.ChangeTracker
            .Entries<AggregationRoot>()
            .Select(x => x.Entity)
            .ToList();

        aggregationRoots
            .ForEach(x =>
            {
                x.IncreaseVersion();
            });

        var domainEvents = aggregationRoots
            .SelectMany(x => x.GetDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        await _resiliencyPolicy.ExecuteAsync(
            async ct =>
            {
                await _context.SaveChangesAsync(ct);
            },
            cancellationToken);

        var integrationEvents = aggregationRoots
            .SelectMany(x => x.GetIntegrationEvents())
            .ToList();

        aggregationRoots
            .ForEach(x =>
            {
                x.ClearDomainEvents();
            });

        foreach (var integrationEvent in integrationEvents)
        {
            await _publisher.Publish(integrationEvent, cancellationToken);
        }
    }
}