using LocalVibe.API.DTOs.Weather;

namespace LocalVibe.API.Services.Interfaces;

public interface IWeatherService
{
    /// <summary>Lấy thời tiết hiện tại tại toạ độ cho trước (Open-Meteo).</summary>
    Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude);
}
