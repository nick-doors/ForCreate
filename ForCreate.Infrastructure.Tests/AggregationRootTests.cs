using ForCreate.Shared.Data;
using ForCreate.Shared.Entities;
using ForCreate.Shared.Events;
using ForCreate.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForCreate.Infrastructure.Tests;

public class AggregationRootTests : IDisposable
{
    private readonly IUnitOfWork _sut;

    private readonly Mock<IPublisher> _publisher = new(MockBehavior.Strict);
    private readonly TestDbContext _context;

    public AggregationRootTests()
    {
        var options = new DbContextOptionsBuilder<ForCreateDbContext>()
            .UseInMemoryDatabase("test")
            .Options;
        _context = new TestDbContext(options);
        _sut = new UnitOfWork<ForCreateDbContext>(_context, ResiliencyPolicy.GetSqlResiliencyPolicy(), _publisher.Object);
    }

    public void Dispose()
    {
        Mock.VerifyAll(_publisher);
    }

    [Fact]
    public async Task UnitOfWork_Should_PublishDomainEvents_When_AggregationRoot_Given()
    {
        var root = new TestAggregationRoot();

        root.GetDomainEvents().Should()
            .BeEmpty();

        var domainEvent = root.RegisterEvent();

        _publisher
            .Setup(x => x.Publish(domainEvent, default))
            .Returns(Task.CompletedTask);

        root.GetDomainEvents().Should()
            .HaveCount(1);

        root.IsVersionIncreased.Should()
            .BeFalse();

        _context.Add(root);

        _context.Saved.Should()
            .BeFalse();

        await _sut.SaveAsync();

        _context.Saved.Should()
            .BeTrue();

        root.IsVersionIncreased.Should()
            .BeTrue();
    }
}

internal class TestDbContext : ForCreateDbContext
{
    public TestDbContext(DbContextOptions<ForCreateDbContext> options)
        : base(options)
    {
    }

    public bool Saved { get; private set; }

    public DbSet<TestAggregationRoot> Roots { get; set; } = default!;

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        Saved = true;
        return Task.FromResult(0);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}

internal class TestAggregationRoot : AggregationRoot
{
    public override void IncreaseVersion()
    {
        IsVersionIncreased = true;
        base.IncreaseVersion();
    }

    public IDomainEvent RegisterEvent()
    {
        var domainEvent = new TestDomainEvent();
        AddDomainEvent(domainEvent);
        return domainEvent;
    }

    public bool IsVersionIncreased { get; private set; }
}

internal record TestDomainEvent : DomainEvent;