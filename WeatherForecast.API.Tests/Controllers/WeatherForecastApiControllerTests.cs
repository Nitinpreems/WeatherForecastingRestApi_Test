using Microsoft.Extensions.Logging;
using WeatherForecast.Infrastructure;
using Moq;
using AutoFixture;
using WeatherForecast.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.API.Tests
{
    [TestClass()]
    public class WeatherForecastApiControllerTests
    {
        private WeatherForecastApiController _weatherForecastController;
        private readonly Mock<ILogger<WeatherForecastApiController>> _loggerMock;
        private readonly Mock<IWeatherForcastRepository> _weatherForcastRepositoryMock;
        private readonly Mock<IWeatherServiceRepository> _weatherServiceRepositoryMock;
        private Fixture _fixture;
        public WeatherForecastApiControllerTests()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<WeatherForecastApiController>>();
            _weatherForcastRepositoryMock = new Mock<IWeatherForcastRepository>();
            _weatherServiceRepositoryMock = new Mock<IWeatherServiceRepository>();
        }

        [TestMethod]
        public async Task GetWeatherForecastById_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(2)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object,_loggerMock.Object);

            var result = await _weatherForecastController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task GetWeatherForecastByNegetiveId_NotFound()
        {
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Get(-2);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task GetWeatherForecastById_NotFound()
        {
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Get(2);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }


        [TestMethod()]
        public async Task GetWeatherForecastByCordinates_Ok()
        {
            var WfData = _fixture.Create<IEnumerable<WeatherForecastDataModel>>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(20, 50)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task GetWeatherForecastByCordinates_NotFound()
        {
            var WfData = _fixture.Create<IEnumerable<WeatherForecastDataModel>>();
            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(20, 50)).ReturnsAsync(value: null);
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);
            var result = await _weatherForecastController.Get(20, 50);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }


        //GetAll
        [TestMethod()]
        public async Task GetAllWeatherForecast_Ok()
        {
            var WfData = _fixture.Create<IEnumerable<WeatherForecastDataModel>>();

            _weatherForcastRepositoryMock.Setup(x => x.GetAllWeatherForecasts(true)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.GetAll(true);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task GetAllWeatherForecast_NotFound()
        {
            _weatherForcastRepositoryMock.Setup(x => x.GetAllWeatherForecasts(true)).ReturnsAsync(value: null);
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.GetAll(true);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }

        //Update
        [TestMethod()]
        public async Task UpdateWeatherForecast_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var WfNewData = _fixture.Create<WeatherForecastDataModel>();
            var request = _fixture.Create<UpdateWeatherForecastByUrlRequest>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(request.Id)).ReturnsAsync(WfData);
            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByUrl(request.url, CancellationToken.None)).ReturnsAsync(WfNewData);
            _weatherForcastRepositoryMock.Setup(x => x.UpdateWeatherForecast(WfData)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Update(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateWeatherForecast_NegetiveId_NotFound()
        {
            var request = _fixture.Create<UpdateWeatherForecastByUrlRequest>();
            request.Id = -1;

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Update(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateWeatherForecast_EmptyUrl_BadRequest()
        {
            var request = _fixture.Create<UpdateWeatherForecastByUrlRequest>();
            request.Id = -1;

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Update(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateWeatherForecast_SameUrl_BadRequest()
        {
            var request = _fixture.Create<UpdateWeatherForecastByUrlRequest>();
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            WfData.Url = request.url;

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(request.Id)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Update(request);
            var obj = result as ObjectResult;

            Assert.AreEqual($"Url is already same, Hence Update Skipped. Id: {request.Id}", obj.Value.ToString());
        }

        [TestMethod()]
        public async Task UpdateWeatherForecastByTimezone_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var WfNewData = _fixture.Create<WeatherForecastDataModel>();
            var request = _fixture.Create<UpdateWeatherForecastByTimeZoneRequest>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(request.Id)).ReturnsAsync(WfData);
            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByUrl(WfData.Url, CancellationToken.None)).ReturnsAsync(WfNewData);
            _weatherForcastRepositoryMock.Setup(x => x.UpdateWeatherForecast(WfData)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.UpdateByTimeZone(request);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateWeatherForecastByTimezone_SameTimeZone_BadRequest()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var request = _fixture.Create<UpdateWeatherForecastByTimeZoneRequest>();
            WfData.Timezone = request.Timezone.ToString();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(request.Id)).ReturnsAsync(WfData);
            _weatherForcastRepositoryMock.Setup(x => x.UpdateWeatherForecast(WfData)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.UpdateByTimeZone(request);
            var obj = result as ObjectResult;
            Assert.AreEqual($"Timezone is already same, Hence Update Skipped. Id:{request.Id}", obj.Value.ToString());
        }

        //Refresh
        [TestMethod()]
        public async Task RefreshWeatherForecastById_Ok()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var WfNewData = _fixture.Create<WeatherForecastDataModel>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(1)).ReturnsAsync(WfData);
            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(WfData.Latitude, WfData.Longitude, CancellationToken.None)).ReturnsAsync(WfNewData);
            _weatherForcastRepositoryMock.Setup(x => x.UpdateWeatherForecast(WfData)).ReturnsAsync(WfData);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Refresh(1);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task RefreshWeatherForecastById_NoDataFromService_NotFound()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();

            _weatherForcastRepositoryMock.Setup(x => x.GetWeatherForecastById(1)).ReturnsAsync(WfData);
            _weatherServiceRepositoryMock.Setup(x => x.GetWeatherForecastDataByCordinates(WfData.Latitude, WfData.Longitude, CancellationToken.None)).ReturnsAsync(value: null);

            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Refresh(1);
            var obj = result as ObjectResult;
            Assert.AreEqual("No Data returned by Weather Service. Update skipped", obj.Value.ToString());
        }

        //Delete
        [TestMethod()]
        public async Task DeleteWeatherForecastById_Ok()
        {
            _weatherForcastRepositoryMock.Setup(x => x.DeleteWeatherForecast(1)).ReturnsAsync(true);
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Delete(1);
            var obj = result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod()]
        public async Task DeleteWeatherForecastById_NotFound()
        {
            _weatherForcastRepositoryMock.Setup(x => x.DeleteWeatherForecast(1)).ReturnsAsync(false);
            _weatherForecastController = new WeatherForecastApiController(_weatherForcastRepositoryMock.Object, _weatherServiceRepositoryMock.Object, _loggerMock.Object);

            var result = await _weatherForecastController.Delete(1);
            var obj = result as ObjectResult;
            Assert.AreEqual(404, obj.StatusCode);
        }
    }
}