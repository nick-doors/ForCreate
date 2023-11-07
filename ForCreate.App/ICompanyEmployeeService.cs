using ForCreate.App.CompanyAggregation.Create;
using ForCreate.App.EmployeeAggregation.Create;
using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;

namespace ForCreate.App;

public interface ICompanyEmployeeService
{
    Task<ICollection<Employee>> GetEmployeesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    Task<ICollection<Company>> GetCompaniesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    ValueTask<ICollection<Employee>> CreateEmployeesAsync(
        ICollection<EmployeeCreate> employees,
        CancellationToken cancellationToken = default);

    ValueTask<Company> CreateCompanyAsync(
        CompanyCreate company,
        CancellationToken cancellationToken = default);
}