using Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParallelForEachAsyncController(IServerError serverErrorClient,
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

                ConcurrentBag<IEnumerable<WeatherForecast>> results = [];

                await Parallel.ForEachAsync(requests, 
                                            new ParallelOptions { MaxDegreeOfParallelism = requests.Length },
                                            async (request, _) => 
                                            {
                                                var result = await request;
                                                results.Add(result);
                                            });

                var data = results.SelectMany(i => i);
                stopwatch.Stop();

                return Ok(new { Elapsed = stopwatch.ElapsedMilliseconds, Data = data });
            }
            catch (Exception ex)
            {
                //Handle ex

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
