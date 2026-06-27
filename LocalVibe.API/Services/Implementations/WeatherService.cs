using LocalVibe.API.DTOs.Weather;
using LocalVibe.API.Services.Interfaces;
using System.Text.Json;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Lấy thời tiết thực tế từ Open-Meteo (https://open-meteo.com).
/// Hoàn toàn miễn phí, không cần API key.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;

    public WeatherService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("OpenMeteo");
    }

    public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
    {
        var url = $"v1/forecast?latitude={latitude}&longitude={longitude}" +
                  "&current=temperature_2m,weathercode,windspeed_10m,precipitation" +
                  "&timezone=auto";

        var json = await _http.GetStringAsync(url);
        using var doc = JsonDocument.Parse(json);

        var current = doc.RootElement.GetProperty("current");

        var temperature   = current.GetProperty("temperature_2m").GetDouble();
        var weatherCode   = current.GetProperty("weathercode").GetInt32();
        var windSpeed     = current.GetProperty("windspeed_10m").GetDouble();
        var precipitation = current.GetProperty("precipitation").GetDouble();

        var (condition, icon) = MapWeatherCode(weatherCode);

        return new WeatherResponse
        {
            Temperature   = temperature,
            Condition     = condition,
            ConditionIcon = icon,
            WindSpeed     = windSpeed,
            Precipitation = precipitation,
            FetchedAt     = DateTime.UtcNow
        };
    }

    // WMO Weather Code → Mô tả + Emoji
    private static (string condition, string icon) MapWeatherCode(int code) => code switch
    {
        0                      => ("Bầu trời quang", "☀️"),
        1                      => ("Ít mây", "🌤️"),
        2                      => ("Có mây", "⛅"),
        3                      => ("Nhiều mây", "☁️"),
        45 or 48               => ("Có sương mù", "🌫️"),
        51 or 53 or 55         => ("Mưa phùn nhẹ", "🌦️"),
        56 or 57               => ("Mưa phùn đóng băng", "🌧️"),
        61 or 63 or 65         => ("Mưa", "🌧️"),
        66 or 67               => ("Mưa đóng băng", "🌨️"),
        71 or 73 or 75         => ("Tuyết rơi", "❄️"),
        77                     => ("Hạt tuyết", "🌨️"),
        80 or 81 or 82         => ("Mưa rào", "🌦️"),
        85 or 86               => ("Mưa tuyết", "🌨️"),
        95                     => ("Dông", "⛈️"),
        96 or 99               => ("Dông kèm mưa đá", "⛈️"),
        _                      => ("Không xác định", "🌡️")
    };
}
