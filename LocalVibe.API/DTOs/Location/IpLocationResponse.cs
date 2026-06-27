namespace LocalVibe.API.DTOs.Location;

/// <summary>Kết quả phát hiện vị trí theo IP (ip-api.com).</summary>
public class IpLocationResponse
{
    public string Ip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
}
