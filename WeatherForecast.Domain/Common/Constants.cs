

namespace WeatherForecast.Domain
{
    public static class Constants
    {
        //Url Constants
        public const string BaseUrl = $"https://api.open-meteo.com/v1/forecast";
        public const string DefaultTimezone = "GMT";
        public const string LatitudeQueryString = "latitude";
        public const string LongitudeQueryString = "longitude";
        public const string DefaultQueryString = "hourly=temperature_2m";
        public const string HourlyQueryString = "hourl";
        public const string DailyQueryString = "daily";
        public const string TimezoneQueryString = "timezone";

        //HourlyDataSetNames
        public const string hDsnTemperature_2m = "temperature_2m";
        public const string dDsnRain = "rain";

        //DailyDataSetNames
        public const string dDsnTemperature_2m_max = "temperature_2m_max";
        public const string dDsnWeathercode = "weathercode";
    }
    
}