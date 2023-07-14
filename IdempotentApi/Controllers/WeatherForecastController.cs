using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdempotentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")] // We should define this.
    [Produces("application/json")] // We should define this.
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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


        [HttpPost(Name = "PostWeatherForcast")]
        [Idempotent(Enabled = true, CacheOnlySuccessResponses = true)]
        public IActionResult Post([FromHeader(Name = "IdempotencyKey")][Required] Guid requiredHeader, [FromBody]WeatherForecast weatherForecast)
        {
            _logger.LogInformation("POST: weatherForcast");
            return Ok(weatherForecast);
        }
    }
}