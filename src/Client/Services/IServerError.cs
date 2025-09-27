using Refit;

namespace Client.Services
{
    public interface IServerError
    {
        [Get("/weatherforecast/{shouldThrow}")]
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(bool shouldThrow = false);
    }
}
