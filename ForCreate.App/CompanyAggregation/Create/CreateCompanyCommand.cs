
using ForCreate.App.EmployeeAggregation.Create;

using MediatR;

namespace ForCreate.App.CompanyAggregation.Create;

public record CreateCompanyCommand : IRequest
{
    public CreateCompanyCommand(
        CompanyCreate company,
        IEnumerable<Guid> existingEmployeesIds,
        IEnumerable<EmployeeCreate> newEmployees)
    {
        NewEmployees = newEmployees
            .ToHashSet();
        ExistingEmployeesIds = existingEmployeesIds
            .ToHashSet();
        Company = company;
    }

    public HashSet<EmployeeCreate> NewEmployees { get; }

    public HashSet<Guid> ExistingEmployeesIds { get; }

    public CompanyCreate Company { get; }
}