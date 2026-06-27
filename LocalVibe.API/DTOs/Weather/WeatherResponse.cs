namespace LocalVibe.API.DTOs.Weather;

public class WeatherResponse
{
    public double Temperature { get; set; }       // °C
    public string Condition { get; set; } = string.Empty;   // "Nắng", "Mưa"...
    public string ConditionIcon { get; set; } = string.Empty; // emoji
    public double WindSpeed { get; set; }          // km/h
    public double Precipitation { get; set; }      // mm
    public string Unit { get; set; } = "°C";
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
}
