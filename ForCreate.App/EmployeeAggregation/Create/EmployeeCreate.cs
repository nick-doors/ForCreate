using ForCreate.Core.Enums;

namespace ForCreate.App.EmployeeAggregation.Create;

public record EmployeeCreate
{
    public required EmployeeTitle Title { get; init; }

    public required string Email { get; init; }
}