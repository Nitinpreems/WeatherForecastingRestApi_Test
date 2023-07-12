using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure.Tests
{
    [TestClass]
    public class WeatherForcastRepositoryTests
    {
        private IWeatherForcastRepository _weatherForcastRepository;
        private Mock<IBaseRepository<WeatherForecastDataModel>> _baseEfCoreRepo;
        private readonly Mock<DbContext> _DbContextMock;
        private readonly Mock<WeatherForecastDbContext> _weatherForecastDbContextMock;
        private Fixture _fixture;
        public WeatherForcastRepositoryTests()
        {
            _fixture = new Fixture();
            _DbContextMock = new Mock<DbContext>();
            _weatherForecastDbContextMock = new Mock<WeatherForecastDbContext>();
            _baseEfCoreRepo = new Mock<IBaseRepository<WeatherForecastDataModel>>();
        }

        

        [TestMethod]
        public async Task GetAllWeatherForecastsTest_Success()
        {
            var WfData = _fixture.Create<IEnumerable<WeatherForecastDataModel>>();

            //_DbContextMock.Setup(c => c.SaveChanges()).Verifiable();
            //_DbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
            ////_DbContextMock.Setup(c => c.Add(It.IsAny<WeatherForecastDataModel>())).Returns(It.IsAny<WeatherForecastDataModel>).Verifiable();

            //_DbContextMock.Setup(p => p.FindAsync<WeatherForecastDataModel>(WfData.Id)).ReturnsAsync(It.IsAny<WeatherForecastDataModel>);
            //_DbContextMock.Setup(p => p.Remove<WeatherForecastDataModel>(WfData)).Returns(value : null);

            //_baseEfCoreRepo = new BaseRepository<WeatherForecastDataModel>(_DbContextMock.Object);

            _baseEfCoreRepo.Setup(c => c.GetAll(true)).ReturnsAsync(WfData);
            _weatherForcastRepository = new WeatherForcastRepository(_baseEfCoreRepo.Object, _weatherForecastDbContextMock.Object);

            var result = await _weatherForcastRepository.GetAllWeatherForecasts(true);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public async Task UpdateWeatherForecastTest_Success()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();
            var lstWfData = _fixture.Create<IEnumerable<WeatherForecastDataModel>>();

            _baseEfCoreRepo.Setup(c => c.FindByCondition(x => x.Latitude == WfData.Latitude && x.Longitude == WfData.Longitude && x.Timezone == WfData.Timezone)).ReturnsAsync(lstWfData);
            foreach (var item in lstWfData)
            {
                _baseEfCoreRepo.Setup(c => c.Update(item)).ReturnsAsync(item);
            }

            _weatherForcastRepository = new WeatherForcastRepository(_baseEfCoreRepo.Object, _weatherForecastDbContextMock.Object);

            var result = await _weatherForcastRepository.AddUpdateWeatherForecast(WfData);
            Assert.AreEqual(result.Longitude, WfData.Longitude);
        }

        [TestMethod]
        public async Task AddWeatherForecastTest_Success()
        {
            var WfData = _fixture.Create<WeatherForecastDataModel>();

            _baseEfCoreRepo.Setup(c => c.FindByCondition(x => x.Latitude == WfData.Latitude && x.Longitude == WfData.Longitude && x.Timezone == WfData.Timezone)).ReturnsAsync(value: null);
            _baseEfCoreRepo.Setup(c => c.Add(WfData)).ReturnsAsync(WfData);

            _weatherForcastRepository = new WeatherForcastRepository(_baseEfCoreRepo.Object, _weatherForecastDbContextMock.Object);

            var result = await _weatherForcastRepository.AddUpdateWeatherForecast(WfData);
            
            Assert.IsTrue(result.Id > 0);
        }
    }
}