using govhack2024_backend.Api;
using Microsoft.AspNetCore.Mvc;

namespace govhack2024_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHomesApi _homesApi;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHomesApi homesApi)
    {
        _logger = logger;
        _homesApi = homesApi;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<GetPropertyResponse> Get()
    {
        var property = await _homesApi.GetProperty("auckland", "te-atatu-peninsula", "594-te-atatu-road");
        return property;
    }
}
