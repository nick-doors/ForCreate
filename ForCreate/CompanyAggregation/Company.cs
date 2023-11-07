namespace ForCreate.CompanyAggregation;

public record Company
{
    public string Name { get; init; } = default!;

    public CompanyEmployee[] Employees { get; init; } = Array.Empty<CompanyEmployee>();
}