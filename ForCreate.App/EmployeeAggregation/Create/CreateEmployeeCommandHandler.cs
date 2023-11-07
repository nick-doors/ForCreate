using ForCreate.Core.CompanyAggregation;
using ForCreate.Core.EmployeeAggregation;
using ForCreate.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForCreate.App.EmployeeAggregation.Create;

internal sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly ICompanyEmployeeService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        ICompanyEmployeeService service,
        IUnitOfWork unitOfWork,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _service = service;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var employees = await _service.CreateEmployeesAsync(new[] { command.Employee }, cancellationToken);

            var employee = employees.Single();

            var companies = await _service.GetCompaniesAsync(command.CompaniesIds, cancellationToken);

            foreach (var company in companies)
            {
                company.HireEmployee(employee);
            }

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception e)
        {
            if (e is not EmployeeDuplicateEmailException && e is not CompanyDuplicateEmployeeTitleException)
                _logger.LogError(e, "Unexpected exception");
            throw;
        }
    }
}