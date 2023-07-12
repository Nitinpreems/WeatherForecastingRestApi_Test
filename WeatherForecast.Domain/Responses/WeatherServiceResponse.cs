
namespace WeatherForecast.Domain
{
    public class WeatherServiceResponse
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public HourlyUnits hourly_units { get; set; }
        public Hourly hourly { get; set; }
        public DailyUnits daily_units { get; set; }
        public Daily daily { get; set; }

    }
    
    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> relativehumidity_2m { get; set; }
        public List<double> rain { get; set; }
        public List<double> showers { get; set; }
        public List<double> snowfall { get; set; }
    }

    public class HourlyUnits
    {
        public string time { get; set; }
        public string temperature_2m { get; set; }
        public string relativehumidity_2m { get; set; }
        public string rain { get; set; }
        public string showers { get; set; }
        public string snowfall { get; set; }
    }

    public class Daily
    {
        public List<string> time { get; set; }
        public List<int> weathercode { get; set; }
        public List<double> temperature_2m_max { get; set; }
        public List<double> temperature_2m_min { get; set; }
        public List<double> apparent_temperature_max { get; set; }
        public List<double> apparent_temperature_min { get; set; }
        public List<string> sunrise { get; set; }
        public List<string> sunset { get; set; }
        public List<double> uv_index_max { get; set; }
        public List<double> rain_sum { get; set; }
        public List<double> showers_sum { get; set; }
        public List<double> snowfall_sum { get; set; }
        public List<double> windspeed_10m_max { get; set; }
        public List<int> winddirection_10m_dominant { get; set; }
        public List<double> shortwave_radiation_sum { get; set; }
    }

    public class DailyUnits
    {
        public string time { get; set; }
        public string weathercode { get; set; }
        public string temperature_2m_max { get; set; }
        public string temperature_2m_min { get; set; }
        public string apparent_temperature_max { get; set; }
        public string apparent_temperature_min { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string uv_index_max { get; set; }
        public string rain_sum { get; set; }
        public string showers_sum { get; set; }
        public string snowfall_sum { get; set; }
        public string windspeed_10m_max { get; set; }
        public string winddirection_10m_dominant { get; set; }
        public string shortwave_radiation_sum { get; set; }
    }

}