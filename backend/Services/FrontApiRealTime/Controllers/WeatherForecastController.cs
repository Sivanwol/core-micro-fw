using Microsoft.AspNetCore.Mvc;

namespace FrontApiRealTime.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger) {
        _logger = logger;
    }

    [HttpGet(Name = "Test")]
    public IEnumerable<string[]> Test() {
        return Summaries.Select(summary => new[]
            { summary, DateTime.Now.AddDays(new Random().Next(-10, 10)).ToString() });
    }
}