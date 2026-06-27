namespace LocalVibe.API.DTOs.Geocoding;

/// <summary>Kết quả từ reverse geocoding (lat/lng → địa chỉ).</summary>
public class ReverseGeocodingResponse
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Road { get; set; }
    public string? Suburb { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
