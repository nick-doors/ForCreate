using ForCreate.Core.EmployeeAggregation;

namespace ForCreate.Core.CompanyAggregation;

public class CompanyEmployee
{
    private CompanyEmployee()
    {
    }

    public CompanyEmployee(Company company, Employee employee)
    {
        Company = company;
        Employee = employee;
    }

    public Company Company { get; private set; } = default!;

    public Employee Employee { get; private set; } = default!;
}