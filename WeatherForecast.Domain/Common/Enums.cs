

namespace WeatherForecast.Domain
{
    public class Enums
    {
        public enum WeatherHourlyDataSet
        {
            temperature_2m,
            relativehumidity_2m,
            rain,
            showers,
            snowfall
        }
        public enum WeatherDailyDataSet
        {
            weathercode,
            temperature_2m_max,
            temperature_2m_min,
            apparent_temperature_max,
            apparent_temperature_min,
            sunrise,
            sunset,
            uv_index_max,
            rain_sum,
            showers_sum,
            snowfall_sum,
            windspeed_10m_max,
            winddirection_10m_dominant,
            shortwave_radiation_sum
        }
        public enum TimeZoneEnum
        {
            auto,
            GMT,
            America__New_York,
            America__Chicago,
            Europe__London,
            Asia__Bangkok
        }
    }
    
}