using ForCreate.App.CompanyAggregation.Create;
using ForCreate.App.EmployeeAggregation.Create;
using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Shared.Entities;
using ForCreate.Shared.Exceptions;

namespace ForCreate.App;

internal sealed class CompanyEmployeeService : ICompanyEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;

    public CompanyEmployeeService(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ICollection<Employee>> GetEmployeesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        var enlisted = ids
            .ToHashSet();

        var employees = await _employeeRepository.ListAsync(enlisted, cancellationToken);

        ValidateIdsAndThrow(enlisted, employees, "Employees");

        return employees;
    }

    public async Task<ICollection<Company>> GetCompaniesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        var enlisted = ids
            .ToHashSet();

        var companies = await _companyRepository.ListAsync(enlisted, cancellationToken);

        ValidateIdsAndThrow(enlisted, companies, "Companies");

        return companies;
    }

    public async ValueTask<ICollection<Employee>> CreateEmployeesAsync(
        ICollection<EmployeeCreate> employees,
        CancellationToken cancellationToken = default)
    {
        await ValidateEmployeesAndThrowAsync(employees, cancellationToken);

        var newEmployee = employees
            .Select(x => Employee.Create(x.Email, x.Title))
            .ToList();

        _employeeRepository.AddRange(newEmployee);

        return newEmployee;
    }

    public async ValueTask<Company> CreateCompanyAsync(
        CompanyCreate company,
        CancellationToken cancellationToken = default)
    {
        await ValidateCompanyAndThrowAsync(company, cancellationToken);

        var newCompany = Company.Create(company.Name);

        _companyRepository.Add(newCompany);

        return newCompany;
    }

    private async Task ValidateCompanyAndThrowAsync(
        CompanyCreate company,
        CancellationToken cancellationToken)
    {
        var existingCompany = await _companyRepository.GetByNameAsync(
            company.Name,
            cancellationToken);

        if (existingCompany != null)
            throw new CompanyDuplicateNameException(company.Name);
    }

    private async Task ValidateEmployeesAndThrowAsync(
        IEnumerable<EmployeeCreate> employees,
        CancellationToken cancellationToken)
    {
        var existingEmployees = await _employeeRepository.GetByEmailAsync(employees
            .Select(x => x.Email), cancellationToken);

        var existingEmails = existingEmployees
            .Select(x => x.Email)
            .ToList();

        if (existingEmails.Any())
            throw new EmployeeDuplicateEmailException(existingEmails);
    }

    private static void ValidateIdsAndThrow<T>(
        IEnumerable<Guid> ids,
        IEnumerable<T> items,
        string objectName)
        where T : AggregationRoot
    {
        var notFoundIds = ids
            .Except(items
                .Select(x => x.Id))
            .ToList();

        if (notFoundIds.Any())
            throw new ObjectNotFoundException(objectName, notFoundIds);
    }
}