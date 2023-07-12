

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Domain
{
    public class WeatherForecastDataModel : BaseModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Generationtime_ms { get; set; }
        public int? Utc_offset_seconds { get; set; }
        public string? Timezone { get; set; }
        public string? Timezone_abbreviation { get; set; }
        public double? Elevation { get; set; }
        public List<HourlyData> HourlyData { get; set; }
        public List<DailyData> DailyData { get; set; }
        public string Url { get; set; }
        public void Update(WeatherForecastDataModel newWeatherForecastDataModel)
        {
            this.Latitude = newWeatherForecastDataModel.Latitude;
            this.Longitude = newWeatherForecastDataModel.Longitude;
            this.Generationtime_ms = newWeatherForecastDataModel.Generationtime_ms;
            this.Utc_offset_seconds = newWeatherForecastDataModel.Utc_offset_seconds;
            this.Timezone = newWeatherForecastDataModel.Timezone;
            this.Timezone_abbreviation = newWeatherForecastDataModel.Timezone_abbreviation;
            this.Elevation = newWeatherForecastDataModel.Elevation;
            this.Url = newWeatherForecastDataModel.Url;
            this.HourlyData = newWeatherForecastDataModel.HourlyData;
            this.DailyData = newWeatherForecastDataModel.DailyData;
            foreach (var item in this.HourlyData)
            {
                item.Update();
            }
            foreach (var item in this.DailyData)
            {
                item.Update();
            }
            base.Update();
        }

    }
    public class HourlyData : BaseModel
    {
        public List<DataSet> DataSets { get; set; }

        public int WeatherForecastDataModelId { get; set; }

        public virtual void Update()
        {
            base.Update();
        }

    }
    public class DailyData : BaseModel
    {
        public List<DataSet> DataSets { get; set; }

        public int WeatherForecastDataModelId { get; set; }

        public override void Update()
        {
            base.Update();
        }
    }
    public class DataSet 
    {
        public int Id { get; set; }
        public string DataSetName { get; set; }
        public string? Unit { get; set; }
        public List<DataValue> DataValues { get; set; }
        public int? HourlyDataId { get; set; }
        public int? DailyDataId { get; set; }

    }

    public class DataValue 
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public double Value { get; set; }
        public int DataSetId { get; set; }

    }

    
}