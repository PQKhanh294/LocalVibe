namespace LocalVibe.API.DTOs.Geocoding;

/// <summary>Một kết quả từ geocoding search (Nominatim).</summary>
public class GeocodingResult
{
    public string DisplayName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Type { get; set; } = string.Empty;   // "restaurant", "road"...
    public string? City { get; set; }
    public string? Country { get; set; }
}
