using AutoBogus;
using ForCreate.App.CompanyAggregation.Create;
using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;

namespace ForCreate.App.Tests;

public class CompanyEmployeeServiceTests : IDisposable
{
    private readonly ICompanyEmployeeService _sut;
    private readonly Mock<IEmployeeRepository> _employeeRepository = new(MockBehavior.Strict);
    private readonly Mock<ICompanyRepository> _companyRepository = new(MockBehavior.Strict);

    public CompanyEmployeeServiceTests()
    {
        _sut = new CompanyEmployeeService(
            _employeeRepository.Object,
            _companyRepository.Object);
    }

    public void Dispose()
    {
        Mock.VerifyAll(
            _employeeRepository,
            _companyRepository);
    }

    [Fact]
    public async Task ValidateCompanyNameAsync_Should_ThrownException_When_DuplicateCompanyName_Given()
    {
        var companyCreate = new AutoFaker<CompanyCreate>().Generate();

        _companyRepository
            .Setup(x => x.GetByNameAsync(companyCreate.Name, default))
            .Returns(Task.FromResult<CompanyByNameDto?>(new CompanyByNameDto
            {
                Id = Guid.NewGuid()
            }));

        var subject = async () => await _sut.CreateCompanyAsync(companyCreate);

        await subject.Should()
            .ThrowExactlyAsync<CompanyDuplicateNameException>()
            .WithMessage($"A company named '{companyCreate.Name}' already exists in the system");
    }
}