using Microsoft.EntityFrameworkCore;
using System.Text;
using WeatherForecast.Domain;
using static WeatherForecast.Domain.Enums;

namespace WeatherForecast.Infrastructure
{
    public class WeatherServiceRepository : IWeatherServiceRepository
    {
        private readonly IWeatherService _weatherService;
        
        public WeatherServiceRepository(IWeatherService weatherService) 
        {
            _weatherService = weatherService;
        }

        public async Task<WeatherForecastDataModel> GetWeatherForecastDataByCordinates(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var weatherServiceRequest = new WeatherServiceRequest();
            weatherServiceRequest.Latitude = latitude;
            weatherServiceRequest.Longitude = longitude;
            string strUrl =  weatherServiceRequest.PrepareHttpUrl();
            var response = await _weatherService.GetWeatherForecastData(strUrl, cancellationToken);
            return response.MapResponseToDto(strUrl);
        }

        public async Task<WeatherForecastDataModel> GetWeatherForecastDataByUrl(string modifiedUrl, CancellationToken cancellationToken)
        {
            var response = await _weatherService.GetWeatherForecastData(modifiedUrl, cancellationToken);
            return response.MapResponseToDto(modifiedUrl);
        }

        public async Task<WeatherForecastDataModel> GetWeatherForecastDataByParams(WeatherServiceRequest weatherServiceRequest, CancellationToken cancellationToken)
        {
            string strUrl = weatherServiceRequest.PrepareHttpUrl();
            var response = await _weatherService.GetWeatherForecastData(strUrl, cancellationToken);
            return response.MapResponseToDto(strUrl);
        }

    }
}
