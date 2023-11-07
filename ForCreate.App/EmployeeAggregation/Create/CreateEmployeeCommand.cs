using MediatR;

namespace ForCreate.App.EmployeeAggregation.Create;

public record CreateEmployeeCommand : IRequest
{
    public CreateEmployeeCommand(
        EmployeeCreate employee,
        IEnumerable<Guid> companiesIds)
    {
        CompaniesIds = companiesIds
            .ToHashSet();
        Employee = employee;
    }

    public HashSet<Guid> CompaniesIds { get; }

    public EmployeeCreate Employee { get; }
}