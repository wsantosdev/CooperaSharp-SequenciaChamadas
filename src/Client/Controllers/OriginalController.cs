using Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OriginalController(IServerError serverErrorClient,
                                    IServerSlow serverSlowClient,
                                    IServerTimeout serverTimeoutClient) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = new List<WeatherForecast>();

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            var errorData = await serverErrorClient.GetWeatherForecasts();
            data.AddRange(errorData);

            var slowData = await serverSlowClient.GetWeatherForecasts();
            data.AddRange(slowData);

            var timeoutData = await serverTimeoutClient.GetWeatherForecasts();
            data.AddRange(timeoutData);

            stopwatch.Stop();

            return Ok(new { Elapsed = stopwatch.ElapsedMilliseconds, Data = data });
        }
    }
}
