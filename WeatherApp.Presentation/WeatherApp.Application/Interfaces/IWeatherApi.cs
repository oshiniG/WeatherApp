using WeatherApp.Application.Models;

namespace WeatherApp.Application.Interfaces
{
    public interface IWeatherApi
    {
        Task<WeatherResponse> FetchWeatherAsync(double latitude, double longitude);
    }

}
