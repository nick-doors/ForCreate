using ForCreate.EmployeeAggregation;

namespace ForCreate.Tests.EmployeeAggregation;

public class Validation
{
    [Fact]
    public void Validate()
    {
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        var employee = new Employee
        {
            CompaniesIds = new[] { guid1, guid2, guid1 },
            Email = "not an email"
        };

        var sut = new EmployeeValidator();

        var subject = sut.Validate(employee);
        subject.IsValid.Should()
            .BeFalse();
        subject.Errors
            .Select(x => x.ErrorMessage)
            .Should()
            .BeEquivalentTo(
                "'Email' is not a valid email address.", 
                $"'Companies Ids' contains non-unique values '{guid1}'.");
    }
}