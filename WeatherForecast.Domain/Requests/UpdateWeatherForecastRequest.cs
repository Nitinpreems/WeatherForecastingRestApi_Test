

using static WeatherForecast.Domain.Enums;

namespace WeatherForecast.Domain
{
    public class UpdateWeatherForecastByTimeZoneRequest
    {
        public int Id { get; set; }
        public TimeZoneEnum Timezone { get; set; }

    }

    public class UpdateWeatherForecastByUrlRequest
    {
        public int Id { get; set; }
        public string url { get; set; }

    }

}