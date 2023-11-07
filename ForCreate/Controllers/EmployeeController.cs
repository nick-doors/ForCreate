using System.Runtime.CompilerServices;
using ForCreate.App.EmployeeAggregation.Create;
using ForCreate.EmployeeAggregation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: InternalsVisibleTo("ForCreate.Tests")]
namespace ForCreate.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(Name = "CreateEmployee")]
    public async Task<ActionResult> Post(
        [FromBody] Employee employee,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateEmployeeCommand(
            new EmployeeCreate
            {
                Email = employee.Email,
                Title = employee.Title
            },
            employee.CompaniesIds);

        await _sender.Send(command, cancellationToken);

        return Ok();
    }
}