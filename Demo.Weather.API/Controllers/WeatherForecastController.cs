using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Demo.Weather.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            _logger.LogInformation($">>> Instantiating {typeof(WeatherForecastController).FullName} class.");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($"Invoked {nameof(WeatherForecastController)}.{nameof(Get)} method.");

            var requestUriString = "http://host.docker.internal:5004/WeatherForecast";

            var envRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";

            if (Boolean.TryParse(envRunningInContainer, out var runningInContainer) && !runningInContainer)
            {
                requestUriString = "http://localhost:5004/WeatherForecast";
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUriString)
            {
                Headers =
                {
                    { HeaderNames.UserAgent, "HttpRequestsSample" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            _logger.LogDebug($"{nameof(httpResponseMessage.StatusCode)}: {httpResponseMessage?.StatusCode}");
            _logger.LogDebug($"{nameof(httpResponseMessage.ReasonPhrase)}: {httpResponseMessage?.ReasonPhrase}");

            List<WeatherForecast> results = new();

            if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var tempResults = await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(contentStream);

                _logger.LogDebug($"{nameof(tempResults)} length is {tempResults?.Count() ?? 0}");

                if (tempResults?.Count() > 0)
                {
                    results.AddRange(tempResults);
                    _logger.LogDebug($"Added {nameof(tempResults)} to {nameof(results)}");
                }
            }

            if (results.Count == 0)
            {
                return NotFound();
            }

            _logger.LogDebug($"Results:\n{JsonSerializer.Serialize(results)}\n");

            return Ok(results);
        }
    }
}