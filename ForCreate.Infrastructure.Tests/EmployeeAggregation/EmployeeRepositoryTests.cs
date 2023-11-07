using ForCreate.Core.EmployeeAggregation;
using ForCreate.Infrastructure.EmployeeAggregation;
using ForCreate.Shared.Infrastructure;

namespace ForCreate.Infrastructure.Tests.EmployeeAggregation;

public class EmployeeRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly IEmployeeRepository _sut;

    public EmployeeRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _sut = new EmployeeRepository(fixture.Context(), fixture.Dapper(), ResiliencyPolicy.GetSqlResiliencyPolicy());
    }

    [Fact]
    public async Task GetByEmailAsync_Should_FoundEmployees_When_KnownEmails_Given()
    {
        var subject = await _sut.GetByEmailAsync(
            _fixture.Employees
                .Select(x => x.Email));

        subject.Should()
            .BeEquivalentTo(
                _fixture.Employees
                    .Select(x => new EmployeeByEmailDto
                    {
                        Email = x.Email,
                        Id = x.Id
                    }));
    }

    [Fact]
    public async Task GetAsync_Should_FoundEmployees_When_KnownIds_Given()
    {
        var subject = await _sut.ListAsync(
            _fixture.Employees
                .Select(x => x.Id));

        subject.Should()
            .BeEquivalentTo(
                _fixture.Employees);
    }

    // Other tests goes here
    // to cover 100% of all methods
}