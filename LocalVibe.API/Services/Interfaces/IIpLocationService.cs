using LocalVibe.API.DTOs.Location;

namespace LocalVibe.API.Services.Interfaces;

public interface IIpLocationService
{
    /// <summary>Phát hiện vị trí người dùng dựa theo IP address (ip-api.com).</summary>
    Task<IpLocationResponse?> DetectAsync(string ipAddress);
}
