using ForCreate.Shared.Data;

namespace ForCreate.Core.CompanyAggregation;

public interface ICompanyRepository : IRootRepository<Company>
{
    public Task<CompanyByNameDto?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}