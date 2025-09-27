using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServerError.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{shouldThrow:bool}")]
        public async Task<IEnumerable<WeatherForecast>> Get(bool shouldThrow = false)
        {
            if(shouldThrow)
                throw new Exception("This is a test exception from the server.");
            else
                await Task.Delay(1_000);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray();
        }
    }
}
