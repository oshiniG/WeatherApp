using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Models;
using WeatherApp.Domain.Enums;

namespace WeatherApp.Infrastructure
{
    public class WeatherApi : IWeatherApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<WeatherApi> _logger;
        public WeatherApi(HttpClient httpClient, IOptions<WeatherApiSettings> weatherApiSettings, ILogger<WeatherApi> logger)
        {
            _httpClient = httpClient;
            _apiKey = weatherApiSettings.Value.ApiKey;
            _logger = logger;
        }

        public async Task<WeatherResponse> FetchWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";


                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Weather API request failed. Status code: {StatusCode}", response.StatusCode);
                    throw new Exception($"Weather API call failed with status code {response.StatusCode}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var responseBody = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(responseBody, options);

                if (weatherResponse == null)
                {
                    _logger.LogError("Failed to deserialize weather data for latitude: {Latitude}, longitude: {Longitude}", latitude, longitude);
                    throw new Exception("Failed to deserialize weather data.");
                }
                // Validate data integrity
                if (weatherResponse?.Main?.Temp < -100 || weatherResponse?.Main?.Temp > 60)
                {
                    _logger.LogError("Received invalid temperature data: {Temperature}", weatherResponse.Main?.Temp);
                    throw new Exception("Received invalid temperature data.");
                }

                var weatherData = new WeatherResponse
                {
                    Temperature = weatherResponse.Main.Temp,
                    WindSpeed = weatherResponse.Wind.Speed,
                    Condition = ParseWeatherCondition(weatherResponse.Weather[0].Main)
                };

                return weatherData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching weather data.");
                throw new Exception("An error occurred while fetching weather data: " + ex.Message);
            }
        }

        private WeatherCondition ParseWeatherCondition(string condition)
        {
            return condition.ToLower() switch
            {
                "clear" => WeatherCondition.Clear,
                "rain" => WeatherCondition.Rain,
                "snow" => WeatherCondition.Snow,
                "clouds" => WeatherCondition.Clouds, 
                "windy" => WeatherCondition.Windy,
                _ => WeatherCondition.Unknown // Default to Unknown if the condition is unrecognized
            };
        }
    }
}
