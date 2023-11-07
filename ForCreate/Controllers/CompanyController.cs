using System.Runtime.CompilerServices;
using ForCreate.App.CompanyAggregation.Create;
using ForCreate.App.EmployeeAggregation.Create;
using ForCreate.CompanyAggregation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: InternalsVisibleTo("ForCreate.Tests")]
namespace ForCreate.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ISender _sender;

    public CompanyController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(Name = "CreateCompany")]
    public async Task<ActionResult> Post(
        [FromBody] Company company,
        CancellationToken cancellationToken = default)
    {
        var employeesMap = company.Employees
            .ToLookup(x => x.Id != null);

        var command = new CreateCompanyCommand(
            new CompanyCreate
            {
                Name = company.Name
            },
            employeesMap[true]
                .Select(x => x.Id!.Value),
            employeesMap[false]
                .Select(x => new EmployeeCreate
                {
                    Email = x.Email!,
                    Title = x.Title!.Value
                }));

        await _sender.Send(command, cancellationToken);

        return Ok();
    }
}