using System.ComponentModel.DataAnnotations;
using ForCreate.Core.Enums;
using ForCreate.Shared.Entities;

namespace ForCreate.Core.EmployeeAggregation;

public sealed class Employee : CreatedAggregationRoot
{
    private Employee()
    {
    }

    public EmployeeTitle Title { get; private set; } = EmployeeTitle.Developer;

    [Required]
    [EmailAddress]
    [StringLength(200, MinimumLength = 1)]
    public string Email { get; private set; } = default!;

    public static Employee Create(string email, EmployeeTitle title)
    {
        var employee = new Employee
        {
            Email = email,
            Title = title
        };
        employee.AddDomainEvent(new EmployeeCreatedEvent(employee));
        return employee;
    }
}