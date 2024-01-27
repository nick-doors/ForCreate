using ForCreate.Core.CompanyAggregation;
using ForCreate.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForCreate.App.CompanyAggregation.Create;

internal sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand>
{
    private readonly ICompanyEmployeeService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCompanyCommandHandler> _logger;

    public CreateCompanyCommandHandler(
        ICompanyEmployeeService service,
        IUnitOfWork unitOfWork,
        ILogger<CreateCompanyCommandHandler> logger)
    {
        _service = service;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CreateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var company = await _service.CreateCompanyAsync(command.Company, cancellationToken);

            var newEmployees = await _service.CreateEmployeesAsync(command.NewEmployees, cancellationToken);

            var existingEmployees = await _service.GetEmployeesAsync(command.ExistingEmployeesIds, cancellationToken);

            foreach (var employee in newEmployees
                         .Concat(existingEmployees))
            {
                company.HireEmployee(employee);
            }

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception e)
        {
            if (e is not CompanyDuplicateNameException && e is not CompanyDuplicateEmployeeTitleException)
                _logger.LogError(e, "Unexpected message");
            throw;
        }
    }
}