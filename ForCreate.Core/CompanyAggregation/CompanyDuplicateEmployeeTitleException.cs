using ForCreate.Core.Enums;
using ForCreate.Shared.Exceptions;

namespace ForCreate.Core.CompanyAggregation;

public class CompanyDuplicateEmployeeTitleException : DefaultException
{
    public CompanyDuplicateEmployeeTitleException(string companyName, EmployeeTitle employeeTitle)
        : base($"The company '{companyName}' already has an employee in the title '{employeeTitle}'")
    {
    }
}