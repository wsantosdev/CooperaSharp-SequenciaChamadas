using Refit;

namespace Client.Services
{
    public interface IServerSlow
    {
        [Get("/weatherforecast")]
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
    }
}
