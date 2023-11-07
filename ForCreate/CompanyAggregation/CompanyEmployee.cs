using ForCreate.Core.Enums;

namespace ForCreate.CompanyAggregation;

public record CompanyEmployee
{
    public Guid? Id { get; init; }

    public string? Email { get; init; }

    public EmployeeTitle? Title { get; init; }
}