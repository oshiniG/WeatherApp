using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;
using WeatherApp.Infrastructure;

namespace WeatherApp.Test
{
    public class WeatherServiceTests
    {
        private readonly Mock<IWeatherApi> _mockWeatherApi;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _mockWeatherApi = new Mock<IWeatherApi>();
            _weatherService = new WeatherService(_mockWeatherApi.Object, Mock.Of<ILogger<WeatherService>>());
        }

        [Fact]
        public async Task GetWeatherAsync_Returns_WeatherResponse_With_Recommendation()
        {
            // Arrange
            var weatherApiResponse = new WeatherResponse
            {
                Temperature = 28,
                Condition = Domain.Enums.WeatherCondition.Clear
            };
            _mockWeatherApi.Setup(api => api.FetchWeatherAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(weatherApiResponse);

            // Act
            var result = await _weatherService.GetWeatherAsync(51.5074, -0.1278);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("It’s a great day for a swim", result.Recommendation);
        }

        [Fact]
        public async Task GetWeatherAsync_Returns_Null_When_No_Data()
        {
            // Arrange
            _mockWeatherApi.Setup(api => api.FetchWeatherAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync((WeatherResponse)null);

            // Act
            var result = await _weatherService.GetWeatherAsync(51.5074, -0.1278);

            // Assert
            Assert.Null(result);
        }
    }

}
