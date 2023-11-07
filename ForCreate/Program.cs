using FluentValidation;
using FluentValidation.AspNetCore;
using ForCreate.App;
using ForCreate.Infrastructure;
using ForCreate.Middleware;
using ForCreate.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(o => o.SerializerSettings.Converters.Add(new StringEnumConverter()));
builder.Services.AddFluentValidationAutoValidation(cfg => { cfg.DisableDataAnnotationsValidation = true; });
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails(cfg =>
{
    cfg.CustomizeProblemDetails = ctx =>
    {
        var exception = ctx.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        if (exception == null)
            return;

        if (exception is DefaultException)
        {
            ctx.ProblemDetails.Title = "One or more errors occurred.";
            ctx.ProblemDetails.Status = StatusCodes.Status400BadRequest;
            ctx.ProblemDetails.Detail = exception.Message;
            ctx.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("ForCreateDb")!);
builder.Services.AddApp();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePages();
app.UseExceptionHandler();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseInfrastructure();

app.Run();