using ForCreate.Core.Enums;

namespace ForCreate.EmployeeAggregation;

public record Employee
{
    public string Email { get; init; } = default!;

    public EmployeeTitle Title { get; init; }

    public Guid[] CompaniesIds { get; init; } = Array.Empty<Guid>();
}