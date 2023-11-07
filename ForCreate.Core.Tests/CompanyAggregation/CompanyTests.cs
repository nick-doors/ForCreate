using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Core.Enums;

namespace ForCreate.Core.Tests.CompanyAggregation;

public class CompanyTests
{
    [Fact]
    public void Ok()
    {
        var subject = Company.Create("name");
        subject.Name.Should()
            .Be("name");

        subject.GetDomainEvents()
            .Cast<CompanyCreatedEvent>()
            .Should()
            .HaveCount(1);

        subject.Employees.Should()
            .BeEmpty();
        subject.HireEmployee(Employee.Create("email_1@a.b", EmployeeTitle.Developer));
        subject.Employees.Should()
            .HaveCount(1);

        var ex = () => subject.HireEmployee(Employee.Create("email_1@a.b", EmployeeTitle.Developer));

        ex.Should()
            .ThrowExactly<CompanyDuplicateEmployeeTitleException>()
            .WithMessage("The company 'name' already has an employee in the title 'Developer'");
    }
}