using Refit;

namespace Client.Services
{
    public interface IServerTimeout
    {
        [Get("/weatherforecast/{shouldTimeout}")]
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(bool shouldTimeout = false);
    }
}
