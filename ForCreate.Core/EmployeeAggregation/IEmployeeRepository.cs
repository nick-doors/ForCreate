using ForCreate.Shared.Data;

namespace ForCreate.Core.EmployeeAggregation;

public interface IEmployeeRepository : IRootRepository<Employee>
{
    public Task<IEnumerable<EmployeeByEmailDto>> GetByEmailAsync(
        IEnumerable<string> emails,
        CancellationToken cancellationToken = default);
}