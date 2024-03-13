using Microsoft.AspNetCore.Mvc;
using weather.Services;

namespace weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpGet("weather/{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        var data = await _service.GetCoordinate(city);
        if(data != null)
        {
            var response = await _service.GetCurrentWeather(data.lat, data.lon, city);
            return Ok(response);
        }
        return BadRequest();
    }
}

