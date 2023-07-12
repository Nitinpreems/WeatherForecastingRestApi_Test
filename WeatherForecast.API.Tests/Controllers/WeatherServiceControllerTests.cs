
using Microsoft.Extensions.Logging;
using WeatherForecast.Infrastructure;
using Moq;
using AutoFixture;
using WeatherForecast.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.API.Tests
{
    [TestClass]
    public class WeatherServiceControllerTests
    {
        private WeatherServiceController _weatherServiceController;
        private readonly Mock<ILogger<WeatherServiceController>> _loggerMock;
        private readonly Mock<IWeatherForcastRepository> _weatherForcastRepositoryMock;
        private readonly Mock<IWeatherServiceRepository> _weatherServiceRepositoryMock;
        private Fixture _fixture;
        public WeatherServiceControllerTests()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<WeatherServiceController>>();
            _weatherForcastRepositoryMock = new Mock<IWeatherForcastRepository>();
            _weatherServiceRepositoryMock = new Mock<IWeatherServiceRepository>();
        }

        [TestMethod]
        public async Task GetWeatherForecastDataByCordinates_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var WfDataAfterAdd = _fixture.Create<WeatherForecastDataModel>();

            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(20, 50, CancellationToken.None)).ReturnsAsync(WfData);
            _weatherForcastRepositoryMock.Setup(x => x.AddUpdateWeatherForecast(WfData)).ReturnsAsync(WfDataAfterAdd);

            _weatherServiceController = new WeatherServiceController(_weatherServiceRepositoryMock.Object, _weatherForcastRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherServiceController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task GetWeatherForecastDataByCordinates_NotFound()
        {
            _weatherServiceController = new WeatherServiceController(_weatherServiceRepositoryMock.Object, _weatherForcastRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherServiceController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task GetWeatherForecastDataByCordinates_BadRequest()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();

            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(20, 50, CancellationToken.None)).ReturnsAsync(WfData);

            _weatherServiceController = new WeatherServiceController(_weatherServiceRepositoryMock.Object, _weatherForcastRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherServiceController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(400, obj.StatusCode);
        }



        [TestMethod]
        public async Task GetWeatherForecastDataByParams_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var WfDataAfterAdd = _fixture.Create<WeatherForecastDataModel>();
            var request = _fixture.Create<WeatherServiceRequest>();

            //var request = new WeatherServiceRequest()
            //{
            //    Latitude = 10,
            //    Longitude = 50,
            //    HourlyDataSets = new List<WeatherHourlyDataSet>() { WeatherHourlyDataSet.temperature_2m },
            //    DailyDataSets = new List<WeatherDailyDataSet>() { WeatherDailyDataSet.temperature_2m_max },
            //    Timezone = TimeZoneEnum.GMT
            //};

            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByParams(request, CancellationToken.None)).ReturnsAsync(WfData);
            _weatherForcastRepositoryMock.Setup(x => x.AddUpdateWeatherForecast(WfData)).ReturnsAsync(WfDataAfterAdd);

            _weatherServiceController = new WeatherServiceController(_weatherServiceRepositoryMock.Object, _weatherForcastRepositoryMock.Object, _loggerMock.Object);
            
            var result = await _weatherServiceController.GetByParam(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task GetWeatherForecastDataByParams_NotFound()
        {
            var request = _fixture.Create<WeatherServiceRequest>();

            _weatherServiceController = new WeatherServiceController(_weatherServiceRepositoryMock.Object, _weatherForcastRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherServiceController.GetByParam(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }
    }
}