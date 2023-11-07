using System.Net;
using System.Text;
using ForCreate.Core.Enums;
using ForCreate.EmployeeAggregation;
using Newtonsoft.Json;

namespace ForCreate.Tests.Controllers;

public class EmployeeControllerTests : IClassFixture<BaseIntegrationTestsFactory>
{
    private readonly HttpClient _client;

    public EmployeeControllerTests(BaseIntegrationTestsFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Ok()
    {
        var payload = new StringContent(
            JsonConvert.SerializeObject(
                new Employee
                {
                    Email = "valid@ema.il",
                    Title = EmployeeTitle.Manager
                }),
            Encoding.UTF8,
            "application/json");
        var response = await _client.PostAsync("/Employee", payload);

        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);
    }
}