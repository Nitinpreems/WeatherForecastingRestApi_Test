using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Domain;
using WeatherForecast.Infrastructure;

namespace WeatherForecast.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastApiController : ControllerBase
    {
        private readonly ILogger<WeatherForecastApiController> _logger;
        private readonly IWeatherForcastRepository _weatherForcastRepository;
        private readonly IWeatherServiceRepository _weatherServiceRepository;

        public WeatherForecastApiController(IWeatherForcastRepository weatherForcastRepository, IWeatherServiceRepository weatherServiceRepository, ILogger<WeatherForecastApiController> logger)
        {
            _weatherForcastRepository = weatherForcastRepository;
            _weatherServiceRepository = weatherServiceRepository;
            _logger = logger;
        }

        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
            {
                return NotFound($"Id not acceptable. Id:{id}");
            }
            var wfData = await _weatherForcastRepository.GetWeatherForecastById(id);
            if (wfData == null)
            {
                return NotFound($"No record found in repository. Id:{id}");
            }
            return Ok(wfData);
        }

        [Route("GetByCordinates")]
        [HttpGet]
        public async Task<IActionResult> Get(double lattitude, double longitude)
        {
            var wfData = await _weatherForcastRepository.GetWeatherForecastDataByCordinates(lattitude, longitude);
            if (wfData == null)
            {
                return NotFound($"No record found in repository. lattitude:{lattitude}, longitude:{longitude}");
            }
            return Ok(wfData);
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll(bool lazyLoad = true)
        {
            //Yield can be used if records are huge.
            var lstWfData = await _weatherForcastRepository.GetAllWeatherForecasts(lazyLoad);
            if (lstWfData == null || lstWfData.Count() < 1)
            {
                return NotFound("No record found in repository");
            }
            return Ok(lstWfData);
        }

        [Route("UpdateByUrl")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateWeatherForecastByUrlRequest updateRequest)
        {
            if (updateRequest == null)
                return BadRequest($"Null Request Object");
            if (updateRequest.Id < 1)
                return NotFound($"Id not acceptable. Id:{updateRequest.Id}");
            if (string.IsNullOrEmpty(updateRequest.url))
                return BadRequest($"Null Url in Request");

            var currentdata = await _weatherForcastRepository.GetWeatherForecastById(updateRequest.Id);

            if (currentdata != null)
            {
                if (updateRequest.url != currentdata.Url)
                {
                    var wfRefreshData = await _weatherServiceRepository.GetWeatherForecastDataByUrl(updateRequest.url, CancellationToken.None);

                    //currentdata.Update(wfRefreshData);
                    //wfRefreshData = await _weatherForcastRepository.UpdateWeatherForecast(currentdata);
                    //if (wfRefreshData.ModifiedDate == null && wfRefreshData.ModifiedDate <= currentdata.ModifiedDate)
                    //    return BadRequest($"Refresh not done. Id: {updateRequest.Id}");
                    return await UpdateRepositoryData(currentdata, wfRefreshData);
                }
                else
                {
                    return BadRequest($"Url is already same, Hence Update Skipped. Id: {updateRequest.Id}");
                }
            }
            else
                return NotFound($"No record Found. Id:{updateRequest.Id}");
        }

        [Route("UpdateByTimeZone")]
        [HttpPut]
        public async Task<IActionResult> UpdateByTimeZone(UpdateWeatherForecastByTimeZoneRequest updateRequest)
        {
            if (updateRequest == null)
                return BadRequest($"Null Request Object");
            if (updateRequest.Id < 1)
                return NotFound($"Id not acceptable. Id:{updateRequest.Id}");

            var currentdata = await _weatherForcastRepository.GetWeatherForecastById(updateRequest.Id);

            if (currentdata != null)
            {
                string newTimeZone = updateRequest.Timezone.GetTimeZoneString();
                if (newTimeZone != currentdata.Timezone)
                {
                    var dicParam = new Dictionary<string, string>();
                    dicParam.Add(Constants.TimezoneQueryString, newTimeZone);
                    string updatedUrl = currentdata.Url.UpdateUrl(dicParam);
                    
                    var wfRefreshData = await _weatherServiceRepository.GetWeatherForecastDataByUrl(updatedUrl, CancellationToken.None);

                    return await UpdateRepositoryData(currentdata, wfRefreshData);
                }
                else
                {
                    return BadRequest($"Timezone is already same, Hence Update Skipped. Id:{updateRequest.Id}");
                }
            }
            else
                return NotFound($"No record Found, Data Update by timezone skipped. Id:{updateRequest.Id}");
            
        }

        [Route("Refresh")]
        [HttpPut]
        public async Task<IActionResult> Refresh(int id)
        {
            if (id < 1)
                return NotFound($"Id not acceptable. Id:{id}");
            var wfCurrentData = await _weatherForcastRepository.GetWeatherForecastById(id);

            if (wfCurrentData != null)
            {
                var wfRefreshData = await _weatherServiceRepository.GetWeatherForecastDataByCordinates(wfCurrentData.Latitude, wfCurrentData.Longitude, CancellationToken.None);
                if (wfRefreshData == null)
                    return NotFound("No Data returned by Weather Service. Update skipped");

                return await UpdateRepositoryData(wfCurrentData, wfRefreshData, true);
            }
            else
            {
                return NotFound($"No record found in repository for refresh. Id:{id}");
            }
        }

        [Route("RefreshList")]
        [HttpPut]
        public async Task<IActionResult> RefreshList([FromBody] int[] idList)
        {

            var failedId = new List<int>();
            var successId = new List<int>();
            var notFoundId = new List<int>();
            foreach (var id in idList)
            {
                var wfCurrentData = await _weatherForcastRepository.GetWeatherForecastById(id);

                if (wfCurrentData != null)
                {
                    var wfRefreshData = await _weatherServiceRepository.GetWeatherForecastDataByCordinates(wfCurrentData.Latitude, wfCurrentData.Longitude, CancellationToken.None);
                    if (wfRefreshData == null)
                    {
                        failedId.Add(id);
                        _logger.LogWarning($"No Data returned for {id} by Weather Service");
                    }
                    wfCurrentData.Update(wfRefreshData);
                    wfRefreshData = await _weatherForcastRepository.UpdateWeatherForecast(wfCurrentData);
                    if (wfRefreshData.ModifiedDate == null && wfRefreshData.ModifiedDate <= wfCurrentData.ModifiedDate)
                    {
                        failedId.Add(id);
                        _logger.LogWarning($"Update Failed for Id: {id}");
                    }
                    else
                    {
                        successId.Add(id);
                    }
                }
                else
                {
                    notFoundId.Add(id);
                    _logger.LogWarning($"No record found in repository for refresh. Id:{id}");
                }
            }

            if (successId.Any())
            {
                return Ok($"Success:[{string.Join(",", successId)}], Fail:[{string.Join(",", failedId)}], NotFound:[{string.Join(",", notFoundId)}]");
            }
            return BadRequest($"Fail:[{string.Join(",", failedId)}], NotFound:[{string.Join(",", notFoundId)}]");
        }

        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return NotFound($"Id not acceptable. Id:{id}");
            var status = await _weatherForcastRepository.DeleteWeatherForecast(id);

            if (!status)
            {
                return NotFound($"Record Not Found. Id: {id}");
            }
            return Ok($"Delete Successfull. Id: {id}");
        }

        private async Task<IActionResult> UpdateRepositoryData(WeatherForecastDataModel currentdata, WeatherForecastDataModel updateddata, bool returnObject = false)
        {
            currentdata?.Update(updateddata);
            updateddata = await _weatherForcastRepository.UpdateWeatherForecast(currentdata);
            if (updateddata.ModifiedDate == null && updateddata.ModifiedDate <= currentdata.ModifiedDate)
                return BadRequest($"Data Update not done. Id: {currentdata.Id}");

            if (returnObject)
                return Ok(updateddata);
            return Ok($"Update Successfull. Id: {currentdata.Id}");
        }
    }
}