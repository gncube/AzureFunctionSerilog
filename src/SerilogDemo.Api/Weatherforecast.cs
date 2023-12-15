using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SerilogDemo.Domain;
using System.Net;

namespace SerilogDemo.Api;

public class Weatherforecast
{
    private readonly ILogger _logger;

    public Weatherforecast(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Weatherforecast>();
    }

    [Function("Weatherforecast")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weatherforecast")] HttpRequestData req)
    {
        _logger.LogInformation("---> {FunctionName} processed a request.", nameof(Weatherforecast));

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

        // Add a log of the forecast
        _logger.LogError("The forecast is {@forecast}", forecast);


        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(forecast);

        return response;
    }
}
