using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Interfaces;

namespace WeatherApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        /// <summary>
        /// Get the weather data and recommendation based on latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>Returns the weather data and a clothing recommendation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                _logger.LogInformation("Received request to get weather data for latitude {Latitude}, longitude {Longitude}", latitude, longitude);

                var weatherData = await _weatherService.GetWeatherAsync(latitude, longitude);

                // Get the weather recommendation based on the fetched data
                var recommendation = _weatherService.GetWeatherRecommendation(weatherData);

                // Return a structured response with weather data and recommendation
                var result = new
                {
                    Temperature = weatherData.Temperature,
                    WindSpeed = weatherData.WindSpeed,
                    Condition = weatherData.Condition,
                    Recommendation = recommendation
                };

                return Ok(result); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting weather data for latitude {Latitude}, longitude {Longitude}", latitude, longitude);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
