using ForCreate.CompanyAggregation;
using ForCreate.Core.Enums;

namespace ForCreate.Tests.CompanyAggregation;

public class Validation
{
    [Fact]
    public void Validate()
    {
        var guid = Guid.NewGuid();
        var employee = new Company
        {
            Name = null!,
            Employees = new[]
            {
                new CompanyEmployee
                {
                    Id = guid,
                    Email = "email@duplica.te",
                    Title = EmployeeTitle.Tester
                },
                new CompanyEmployee
                {
                    Id = guid,
                    Email = "email@duplica.te",
                    Title = EmployeeTitle.Tester
                },
                new CompanyEmployee
                {
                    Email = "not an email",
                    Title = EmployeeTitle.Developer
                }
            }
        };

        var sut = new CompanyValidator();

        var subject = sut.Validate(employee);
        subject.IsValid.Should()
            .BeFalse();
        subject.Errors
            .Select(x => x.ErrorMessage)
            .Should()
            .BeEquivalentTo(
                "'Name' must not be empty.", 
                "'Email' is not a valid email address.", 
                $"'Employees' contains non-unique ID values '{guid}'.",
                "'Employees' contains non-unique emails 'email@duplica.te'.", 
                "'Employees' contains non-unique titles 'Tester'.");
    }
}