using WeatherApp.Application.Models;

namespace WeatherApp.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude);
        string GetWeatherRecommendation(WeatherResponse weatherData);
    }

}
