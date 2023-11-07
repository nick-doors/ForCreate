namespace ForCreate.Core.EmployeeAggregation;

public record EmployeeByEmailDto
{
    public required Guid Id { get; init; }

    public required string Email { get; init; }
}