
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public interface IWeatherForcastRepository 
    {
        public Task<IEnumerable<WeatherForecastDataModel>> GetAllWeatherForecasts(bool lazyLoad = true);

        public Task<WeatherForecastDataModel> GetWeatherForecastById(int id);

        public Task<WeatherForecastDataModel> AddUpdateWeatherForecast(WeatherForecastDataModel WeatherForecastDto);

        public Task<WeatherForecastDataModel> UpdateWeatherForecast(WeatherForecastDataModel weatherForecastDto);

        public Task<bool> DeleteWeatherForecast(int id);

        public Task<IEnumerable<WeatherForecastDataModel>> GetWeatherForecastDataByCordinates(double lattitude, double longitude);


    }
}
