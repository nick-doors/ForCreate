using AutoBogus;
using ForCreate.App.CompanyAggregation.Create;
using ForCreate.App.EmployeeAggregation.Create;
using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Core.Enums;
using ForCreate.Shared.Data;
using Microsoft.Extensions.Logging;

namespace ForCreate.App.Tests.CompanyAggregation;

public class CreateCompanyCommandHandlerTests : IDisposable
{
    private readonly CreateCompanyCommandHandler _sut;
    private readonly Mock<ICompanyEmployeeService> _companyEmployeeService = new(MockBehavior.Strict);
    private readonly Mock<IUnitOfWork> _unitOfWork = new(MockBehavior.Strict);

    public CreateCompanyCommandHandlerTests()
    {
        _sut = new CreateCompanyCommandHandler(
            _companyEmployeeService.Object,
            _unitOfWork.Object,
            new Mock<ILogger<CreateCompanyCommandHandler>>().Object);
    }

    public void Dispose()
    {
        Mock.VerifyAll(
            _companyEmployeeService,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_CreateCompany()
    {
        var newCompanyCreate = new AutoFaker<CompanyCreate>().Generate();
        var newEmployeeCreate = new AutoFaker<EmployeeCreate>()
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.Title, EmployeeTitle.Developer)
            .Generate();

        var subject = Company.Create(newCompanyCreate.Name);

        var existingEmployee = Employee.Create("ivanov.ivan@mail.com", EmployeeTitle.Tester);
        var newEmployee = Employee.Create(newEmployeeCreate.Email, newEmployeeCreate.Title);

        _companyEmployeeService
            .Setup(x => x.GetEmployeesAsync(new[] { existingEmployee.Id }, default))
            .Returns(Task.FromResult<ICollection<Employee>>(new[] { existingEmployee }));

        _companyEmployeeService
            .Setup(x => x.CreateCompanyAsync(newCompanyCreate, default))
            .Returns(ValueTask.FromResult(subject));

        _companyEmployeeService
            .Setup(x => x.CreateEmployeesAsync(new[] { newEmployeeCreate }, default))
            .Returns(ValueTask.FromResult<ICollection<Employee>>(new[] { newEmployee }));

        _unitOfWork
            .Setup(x => x.SaveAsync(default))
            .Returns(Task.CompletedTask);

        await _sut.Handle(
            new CreateCompanyCommand(
                newCompanyCreate,
                new[] { existingEmployee.Id },
                new[] { newEmployeeCreate }), default);

        subject.Employees
            .Select(x => x.Employee)
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    existingEmployee,
                    newEmployee
                });
    }

    // Other tests goes here
    // to cover 100% of a method
}