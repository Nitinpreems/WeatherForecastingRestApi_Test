using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public class WeatherForcastRepository : BaseRepository<WeatherForecastDataModel> , IWeatherForcastRepository
    {
        private readonly IBaseRepository<WeatherForecastDataModel> _baseEfCoreRepo;
        public WeatherForcastRepository(IBaseRepository<WeatherForecastDataModel> baseEfCoreRepo, WeatherForecastDbContext weatherForecastDbContext) : base(weatherForecastDbContext)
        {
            _baseEfCoreRepo = baseEfCoreRepo;
        }

        public async Task<IEnumerable<WeatherForecastDataModel>> GetAllWeatherForecasts(bool lazyLoad = true)
        {
            if (lazyLoad)
            {
                return await _baseEfCoreRepo.GetAll(lazyLoad);
            }
            else
            {
                return await GetCompleteData();
            }
        }

        public async Task<WeatherForecastDataModel> AddUpdateWeatherForecast(WeatherForecastDataModel weatherForecastDataModel)
        {
            var currentData = await _baseEfCoreRepo.FindByCondition(x => x.Latitude == weatherForecastDataModel.Latitude && x.Longitude == weatherForecastDataModel.Longitude && x.Timezone == weatherForecastDataModel.Timezone); 
            if(currentData != null && currentData.Any())
            {
                var lstResponse = new List<WeatherForecastDataModel>();
                foreach (var item in currentData)
                {
                    item.Update(weatherForecastDataModel);
                    var updatedItem = await UpdateWeatherForecast(item);
                    lstResponse.Add(updatedItem);
                }
                return lstResponse?.FirstOrDefault();
            }
            else
            {
                return await _baseEfCoreRepo.Add(weatherForecastDataModel);
            }
        }

        public async Task<WeatherForecastDataModel> GetWeatherForecastById(int id)
        {
            var response = await GetCompleteData();
            return response.Where(x => x.Id == id).FirstOrDefault(); 
        }

        public async Task<WeatherForecastDataModel> UpdateWeatherForecast(WeatherForecastDataModel weatherForecastDataModel)
        {
            weatherForecastDataModel.ModifiedDate = DateTime.Now;
            return await _baseEfCoreRepo.Update(weatherForecastDataModel);
        }

        public async Task<bool> DeleteWeatherForecast(int Id)
        {
            return await _baseEfCoreRepo.Delete(Id);
        }

        public Task<IEnumerable<WeatherForecastDataModel>> GetWeatherForecastDataByCordinates(double lattitude, double longitude)
        {
            return _baseEfCoreRepo.FindByCondition(x => x.Latitude == lattitude && x.Longitude == longitude);
        }
        
        private async Task<List<WeatherForecastDataModel>> GetCompleteData()
        {
            return await _baseEfCoreRepo.GetDbContext().Set<WeatherForecastDataModel>()
                .Include(x => x.HourlyData)
                .ThenInclude(x => x.DataSets)
                .ThenInclude(x => x.DataValues)
                .Include(x => x.DailyData)
                .ThenInclude(x => x.DataSets)
                .ThenInclude(x => x.DataValues)
                .ToListAsync();
        }
    }
}
