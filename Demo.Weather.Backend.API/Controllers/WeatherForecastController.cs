using Demo.Weather.Backend.API.Database;
using Demo.Weather.Backend.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Weather.Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDatbaseProvider _databaseProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatbaseProvider datbaseProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _databaseProvider = datbaseProvider ?? throw new ArgumentNullException(nameof(datbaseProvider));

            _logger.LogInformation($">>> Instantiating {typeof(WeatherForecastController).FullName} class.");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($">>> Invoked {nameof(WeatherForecastController)}.{nameof(Get)} method.");

            var databaseDatetime = await _databaseProvider.GetDateTime();

            _logger.LogInformation($">>> Datebase DateTime: {databaseDatetime}");

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = databaseDatetime ?? DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }
    }
}