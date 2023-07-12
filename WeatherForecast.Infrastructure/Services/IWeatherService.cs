using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public interface IWeatherService
    {
        Task<WeatherServiceResponse> GetWeatherForecastData(string url, CancellationToken cancellationToken);

    }
}
