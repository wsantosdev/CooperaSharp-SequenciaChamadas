using Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionController(IServerError serverErrorClient,
                                    IServerSlow serverSlowClient,
                                    IServerTimeout serverTimeoutClient) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var requests = new Task<IEnumerable<WeatherForecast>>[] { serverErrorClient.GetWeatherForecasts(),
                                                                      serverSlowClient.GetWeatherForecasts(),
                                                                      serverTimeoutClient.GetWeatherForecasts() };

            try
            {
                var stopwatch = Stopwatch.StartNew();
                stopwatch.Start();

                var results = await Task.WhenAll(requests);
                var data = results.SelectMany(i => i);

                stopwatch.Stop();

                return Ok(new { Elapsed = stopwatch.ElapsedMilliseconds, Data = data });
            }
            catch (AggregateException ex)
            {
                //Handle inner exceptions on ex

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
