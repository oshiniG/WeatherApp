using Microsoft.Extensions.Logging;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Models;
using WeatherApp.Domain.Enums;

namespace WeatherApp.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApi _weatherApi;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IWeatherApi weatherApi, ILogger<WeatherService> logger)
        {
            _weatherApi = weatherApi;
            _logger = logger;
        }

        /// <summary>
        /// Fetches weather data asynchronously based on the provided latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location for which the weather data is being requested.</param>
        /// <param name="longitude">The longitude of the location for which the weather data is being requested.</param>
        /// <returns>
        /// weather details for the given location
        /// </returns>
        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            _logger.LogInformation("Fetching weather data for latitude {Latitude}, longitude {Longitude}", latitude, longitude);
            var weatherResponse  =  await _weatherApi.FetchWeatherAsync(latitude, longitude);
            if (weatherResponse == null)
            {
                _logger.LogWarning("Weather data not found for latitude {Latitude}, longitude {Longitude}", latitude, longitude);
                return null;
            }
            weatherResponse.Recommendation = GetWeatherRecommendation(weatherResponse);
            _logger.LogInformation("Weather data fetched successfully: {Temperature}°C, {Condition}", weatherResponse.Temperature, weatherResponse.Condition);
            return weatherResponse;
        }

        /// <summary>
        /// Provides a weather recommendation based on the given weather data.
        /// </summary>
        /// <param name="weatherData">The weather data including temperature and weather condition.</param>
        /// <returns>
        /// A string message with a weather recommendation based on the condition and temperature
        /// </returns>
        public string GetWeatherRecommendation(WeatherResponse weatherData)
        {
            switch (weatherData.Condition)
            {
                case WeatherCondition.Clear when weatherData.Temperature > 25:
                    return "It’s a great day for a swim"; 

                case WeatherCondition.Rain:
                case WeatherCondition.Snow:
                    if (weatherData.Temperature < 15)
                        return "Don't forget to bring a coat"; 
                    return "Don't forget the umbrella"; 

                case WeatherCondition.Clouds:
                    return "It might be cloudy, but don't let that stop you!"; 

                case WeatherCondition.Clear:
                    return "Don't forget to bring a hat"; 

                default:
                    return "Check the weather before heading out!"; 
            }
        }

    }

}
