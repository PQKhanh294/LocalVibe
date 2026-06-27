using LocalVibe.API.DTOs.Location;
using LocalVibe.API.Services.Interfaces;
using System.Text.Json;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Phát hiện vị trí theo IP dùng ip-api.com.
/// Miễn phí, không cần key, 45 req/phút, chỉ hỗ trợ HTTP (không phải HTTPS) trên free tier.
/// Vì đây là server-to-server call (backend gọi, không phải browser) nên HTTP là chấp nhận được.
/// </summary>
public class IpLocationService : IIpLocationService
{
    private readonly HttpClient _http;

    public IpLocationService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("IpApi");
    }

    public async Task<IpLocationResponse?> DetectAsync(string ipAddress)
    {
        // Nếu là loopback (dev local), trả null thay vì gọi API
        if (string.IsNullOrWhiteSpace(ipAddress) ||
            ipAddress is "::1" or "127.0.0.1" or "localhost")
            return null;

        var url = $"json/{ipAddress}?fields=status,message,country,regionName,city,lat,lon,timezone";

        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            // ip-api trả status "fail" nếu IP không hợp lệ
            if (doc.RootElement.TryGetProperty("status", out var status) &&
                status.GetString() == "fail")
                return null;

            return new IpLocationResponse
            {
                Ip        = ipAddress,
                Country   = doc.RootElement.TryGetProperty("country",    out var c)  ? c.GetString()  ?? "" : "",
                Region    = doc.RootElement.TryGetProperty("regionName", out var r)  ? r.GetString()  ?? "" : "",
                City      = doc.RootElement.TryGetProperty("city",       out var ci) ? ci.GetString() ?? "" : "",
                Latitude  = doc.RootElement.TryGetProperty("lat",        out var la) ? la.GetDouble()      : 0,
                Longitude = doc.RootElement.TryGetProperty("lon",        out var lo) ? lo.GetDouble()      : 0,
                Timezone  = doc.RootElement.TryGetProperty("timezone",   out var tz) ? tz.GetString() ?? "" : ""
            };
        }
        catch
        {
            return null;
        }
    }
}
