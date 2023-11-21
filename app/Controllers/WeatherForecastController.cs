using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Serilog;

namespace Superhero.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly Counter WeatherForeCastCount = Metrics.CreateCounter("weatherforecast_total", "Number of WeatherForecast has ever executed.");

    //private readonly ILogger<WeatherForecastController> _logger;

    
    public WeatherForecastController()
    {
        
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        Log.Information("GetWeatherForecast : " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));
        WeatherForeCastCount.Inc();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
