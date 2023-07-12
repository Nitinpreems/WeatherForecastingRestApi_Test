using WeatherForecast.Domain;
using static WeatherForecast.Domain.Enums;

namespace WeatherForecast.Infrastructure
{
    public interface IWeatherServiceRepository
    {
        Task<WeatherForecastDataModel> GetWeatherForecastDataByCordinates(double latitude, double longitude, CancellationToken cancellationToken);
        Task<WeatherForecastDataModel> GetWeatherForecastDataByUrl(string modifiedUrl, CancellationToken cancellationToken);
        Task<WeatherForecastDataModel> GetWeatherForecastDataByParams(WeatherServiceRequest weatherServiceResponse, CancellationToken cancellationToken);
    }
}