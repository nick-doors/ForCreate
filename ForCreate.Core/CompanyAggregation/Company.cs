using System.ComponentModel.DataAnnotations;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Shared.Entities;

namespace ForCreate.Core.CompanyAggregation;

public sealed class Company : CreatedAggregationRoot
{
    private Company()
    {

    }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; private set; } = default!;

    private readonly HashSet<CompanyEmployee> _employees = new();
    public IReadOnlySet<CompanyEmployee> Employees => _employees;

    public void HireEmployee(Employee employee)
    {
        if (_employees.Any(x => x.Employee.Id == employee.Id))
            return;

        if (_employees.Any(x => x.Employee.Title == employee.Title))
        {
            throw new CompanyDuplicateEmployeeTitleException(Name, employee.Title);
        }

        _employees.Add(new CompanyEmployee(this, employee));
    }

    public static Company Create(string name)
    {
        var company = new Company
        {
            Name = name
        };
        company.AddDomainEvent(new CompanyCreatedEvent(company));
        return company;
    }
}