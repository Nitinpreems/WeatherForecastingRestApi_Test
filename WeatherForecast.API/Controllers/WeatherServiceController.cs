using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecast.Domain;
using WeatherForecast.Infrastructure;

namespace WeatherForecast.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherServiceController : ControllerBase
    {
        private readonly ILogger<WeatherServiceController> _logger;
        private readonly IWeatherForcastRepository _weatherForcastRepository;
        private readonly IWeatherServiceRepository _weatherServiceRepository;

        public WeatherServiceController(IWeatherServiceRepository weatherServiceRepository, IWeatherForcastRepository weatherForcastRepository, ILogger<WeatherServiceController> logger)
        {
            _weatherForcastRepository = weatherForcastRepository;
            _weatherServiceRepository = weatherServiceRepository;
            _logger = logger;
        }

        [Route("Fetch")]
        [HttpGet]
        public async Task<IActionResult> Get(double latitude, double longitude)
        {
            
            var wfData = await _weatherServiceRepository.GetWeatherForecastDataByCordinates(latitude, longitude, CancellationToken.None);
            if (wfData == null)
            {
                return NotFound($"No data found.");
            }
            
            var resultData = await _weatherForcastRepository.AddUpdateWeatherForecast(wfData);
            if (resultData == null)
            {
                return BadRequest($"No record Added or Updated. latitude:{latitude}, longitude:{longitude}");
            }
            _logger.LogInformation($"Data Saved In Memory DB. Id: {wfData.Id}");
            return Ok(resultData);
        }

        [Route("FecthByParam")]
        [HttpPost]
        public async Task<IActionResult> GetByParam([FromBody] WeatherServiceRequest weatherServiceRequest)
        {
            var wfData = await _weatherServiceRepository.GetWeatherForecastDataByParams(weatherServiceRequest, CancellationToken.None);
            if (wfData == null)
            {
                return NotFound($"No data found.");
            }
            var resultData = await _weatherForcastRepository.AddUpdateWeatherForecast(wfData);
            if (resultData == null)
            {
                return BadRequest($"No record Added or Updated. requestObject:{JsonConvert.SerializeObject(weatherServiceRequest)}");
            }
            _logger.LogInformation($"Data Saved In Memory DB. Id: {wfData.Id}");
            return Ok(resultData);
        }


        
    }
}